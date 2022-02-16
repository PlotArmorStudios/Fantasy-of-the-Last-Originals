using UnityEngine;
using UnityEngine.AI;

public class Idle : IState
{
    private readonly Entity _entity;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    public Idle(Entity entity)
    {
        _entity = entity;
        _animator = _entity.Animator;
        _navMeshAgent = _entity.NavAgent;
    }
    public void Tick()
    {
        PlayIdleAnimations();
    }

    public void OnEnter()
    {
        Debug.Log("Navmesh disabled");
        _navMeshAgent.enabled = false;
        _animator.SetBool("Running", false);
    }

    public void OnExit()
    {
    }
    
    void PlayIdleAnimations()
    {
        Debug.Log("Idle");
        _navMeshAgent.enabled = false;
    }
}
