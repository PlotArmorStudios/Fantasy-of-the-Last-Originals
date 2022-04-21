using UnityEngine;
using UnityEngine.AI;

public class Idle : IState
{
    private readonly Entity _entity;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidbody;
    
    private float _returnHomeTimer;
    private float _returnHomeTime;
    private float _canChaseTime;
    private float _canChaseTimer = 1f;
    private readonly FiniteStateMachine _stateMachine;
    private readonly StanceToggler _stanceToggler;
    private readonly CombatManager _combatManager;

    public Idle(FiniteStateMachine stateMachine)
    {
        _entity = stateMachine.GetComponent<Entity>();
        _animator = _entity.Animator;
        _navMeshAgent = _entity.NavAgent;
        _returnHomeTime = _entity.StateMachine.ReturnHomeTime;
        _rigidbody = _entity.Rigidbody;
        
        _stateMachine = stateMachine;
        _animator = stateMachine.GetComponentInChildren<Animator>();
        _stanceToggler = stateMachine.GetComponent<StanceToggler>();
        _combatManager = stateMachine.GetComponent<CombatManager>();
        _stanceToggler.OnStanceChanged += ChangeStance;
    }

    private void ChangeStance(int stance)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            _animator.CrossFade("Stance " + stance, .25f, 0, 0f, 0f);
    }
    
    public void Tick()
    {
        UpdateReturnHomeTime();
        UpdateCanChaseTime();
    }

    public void OnEnter()
    {
        _returnHomeTimer = 0;
        _navMeshAgent.enabled = false;
        _animator.SetBool("Running", false);
        _entity.StateMachine.CanChase = false;
        _canChaseTime = 0;
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Attack 2", false);
        _animator.SetBool("Attack 3", false);

        _combatManager.InputCount = 0;
    }

    public void OnExit()
    {
    }
    
    public bool UpdateReturnHomeTime()
    {
        _returnHomeTimer += Time.deltaTime;
        if (_returnHomeTimer >= _returnHomeTime)
        {
            _returnHomeTimer = _returnHomeTime;
            return true;
        }

        return false;
    }
    
    private void UpdateCanChaseTime()
    {
        _canChaseTime += Time.deltaTime;
        
        if (_canChaseTime >= _canChaseTimer)
        {
            _entity.StateMachine.CanChase = true;
            _canChaseTime = 0;
        }
    }
}
