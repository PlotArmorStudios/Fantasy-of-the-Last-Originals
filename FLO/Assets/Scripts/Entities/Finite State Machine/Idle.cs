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

    public Idle(Entity entity)
    {
        _entity = entity;
        _animator = _entity.Animator;
        _navMeshAgent = _entity.NavAgent;
        _returnHomeTime = _entity.StateMachine.ReturnHomeTime;
        _rigidbody = _entity.Rigidbody;
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
