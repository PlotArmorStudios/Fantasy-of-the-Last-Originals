using UnityEngine;
using UnityEngine.AI;
//State for decision making across the attack, guard, and dodge states
public class OnGuard : IState
{
    private readonly Entity _entity;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly Player _player;
    private EntityStateMachine _stateMachine;
    private float _attackTimer;
    private float _attackDelay;
    private EntityCombatManager _combatManager;
    private AnimatorStateInfo _stateInfo;
    private CurrentAnimatorState _animatorState;
    private EntityStanceToggler _stanceToggler;
    private bool _crossFaded;

    public OnGuard(FiniteStateMachine stateMachine)
    {
        _stateMachine = stateMachine as EntityStateMachine;
        _combatManager = (EntityCombatManager) _stateMachine.GetComponent<CombatManager>();
        _animatorState = _stateMachine.GetComponent<CurrentAnimatorState>();
        _stanceToggler = (EntityStanceToggler) _stateMachine.GetComponent<StanceToggler>();
        _entity = _stateMachine.Entity;
        _player = _stateMachine.Player;
        _animator = _entity.Animator;
        _attackTimer = 4.5f;
    }

    public void Tick()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        AttackPlayer();

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
    }

    public void OnExit()
    {
        _attackTimer = 4.5f;
    }

    private void AttackPlayer()
    {
        if (_attackTimer < _attackDelay)
            _attackTimer += Time.deltaTime;


        _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation,
            Quaternion.LookRotation(_player.transform.position - _entity.transform.position), 5f * Time.deltaTime);

        if (_attackTimer >= _attackDelay)
        {
            _combatManager.TriggerAttack = true;
            Debug.Log("Trigger attack");
            _attackTimer = 0;
        }
    }
}