using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//State for decision making across the attack, guard, and dodge states
[System.Serializable]
public class OnGuard : IState
{
    private readonly Entity _entity;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly Player _player;
    private Rigidbody _rigidbody;

    private EntityStateMachine _stateMachine;
    private float _attackTimer;
    private float _attackDelay;
    private EntityCombatManager _combatManager;
    private AnimatorStateInfo _stateInfo;
    private CurrentAnimatorState _animatorState;
    private EntityStanceToggler _stanceToggler;
    private bool _crossFaded;

    [SerializeField] private float _movementX;
    [SerializeField] private float _movementY;
    [SerializeField] float _xSpeed = 2f;
    [SerializeField] float _ySpeed = 1f;

    [Header("Movement Calculation")] [SerializeField]
    private float _movementXInterval = 1f;
    private float _currentMovementXTime;

    [SerializeField] private float _movementYInterval = 1f;
    private float _strafeDistance => _stateMachine.GuardRadius - .6f;
    private float _currentMovementYTime;
    private float _stopXInterval;

    public OnGuard(FiniteStateMachine stateMachine)
    {
        _stateMachine = stateMachine as EntityStateMachine;
        _combatManager = (EntityCombatManager) _stateMachine.GetComponent<CombatManager>();
        _animatorState = _stateMachine.GetComponent<CurrentAnimatorState>();
        _stanceToggler = (EntityStanceToggler) _stateMachine.GetComponent<StanceToggler>();
        _rigidbody = stateMachine.GetComponent<Rigidbody>();

        _entity = _stateMachine.Entity;
        _player = _stateMachine.Player;
        _animator = _entity.Animator;
        _attackTimer = 4.5f;
    }

    public void Tick()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        FocusPlayer();
        ComputeAttackBehavior();
        ComputeStrafeBehavior();
    }

    private void ComputeStrafeBehavior()
    {
        ComputeXMovement();
        ComputeYMovement();

        if (_movementX > 0.1f && _movementX <= 1f) _movementX = 1f;
        if (_movementX > -1f && _movementX <= -0.1f) _movementX = -1f;
        if (_movementY > 0.1f && _movementY <= 1f) _movementY = 1f;
        if (_movementY > -1f && _movementY <= -0.1f) _movementY = -1f;

        _rigidbody.MovePosition(_rigidbody.position +
                                (_entity.transform.right * _movementX * _xSpeed * Time.deltaTime));
        _animator.SetFloat("On Guard Movement X", _movementX, .2f, Time.deltaTime);

        if (_movementY < 0)
            if (_stateMachine.DistanceToPlayer >= _stateMachine.GuardRadius - .5f)
                return;
        _rigidbody.MovePosition(_rigidbody.position +
                                (_entity.transform.forward * _movementY * _ySpeed * Time.deltaTime));
        _animator.SetFloat("On Guard Movement Y", _movementY, .2f, Time.deltaTime);
    }

    private void ComputeXMovement()
    {
        _movementX = Mathf.Clamp(_movementX, -1f, 1f);

        _currentMovementXTime += Time.deltaTime;

        if (_currentMovementXTime >= _movementXInterval)
        {
            _movementX = Random.Range(-1f, 1f);
            _stateMachine.InjectCoroutine(StopXMovementInIntervals());
            _movementXInterval = Random.Range(.5f, 4f);
            _currentMovementXTime = 0;
        }
    }

    private IEnumerator StopXMovementInIntervals()
    {
        _stopXInterval = Random.Range(.5f, 2f);
        yield return new WaitForSeconds(_stopXInterval);
        _movementX = 0;
    }

    private void ComputeYMovement()
    {
        _movementY = Mathf.Clamp(_movementY, -1f, 1f);
        
        if (_stateMachine.DistanceToPlayer > _strafeDistance + .2f)
            _movementY = 1f;
        if (_stateMachine.DistanceToPlayer < _strafeDistance - .2f)
            _movementY = -1f;
        if (_stateMachine.DistanceToPlayer <= _strafeDistance + .1f &&
            _stateMachine.DistanceToPlayer >= _strafeDistance - .1f)
            _movementY = 0;
    }


    private void ComputeAttackBehavior()
    {
        ComputerAttackInterval();

        if (_combatManager.InputCount >= 1)
        {
            for (int i = 1; i <= _combatManager.InputCount; i++)
            {
                _animator.SetBool($"Attack {i}", true);
            }

            _animator.SetBool("Attacking", true);

            _animator.CrossFade($"S{_stanceToggler.CurrentStance} Attack 1", 0f, 0, 0f);
            _stateMachine.AttackPhase = true;

            _combatManager.ReceiveInput();
            _combatManager.InputReceived = false;
        }
    }

    public void OnEnter()
    {
        _animator.SetBool("Running", false);
        _crossFaded = false;
        _combatManager.InputCount = 0;
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Attack 2", false);
        _animator.SetBool("Attack 3", false);
        _attackTimer = 0;
        _attackDelay = _entity.StateMachine.AttackDelay;

        _animator.CrossFade("On Guard", .25f, 0, 0f);
    }

    public void OnExit()
    {
        _attackTimer = 4.5f;
        _movementX = 0;
        _animator.SetFloat("On Guard Movement X", 0);

        _movementY = 0;
        _animator.SetFloat("On Guard Movement Y", 0);
    }

    private void ComputerAttackInterval()
    {
        if (_attackTimer < _attackDelay)
            _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackDelay)
        {
            _combatManager.TriggerAttack = true;
            Debug.Log("Trigger attack");
            _attackTimer = 0;
        }
    }

    private void FocusPlayer()
    {
        _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation,
            Quaternion.LookRotation(_player.transform.position - _entity.transform.position), 5f * Time.deltaTime);
    }
}