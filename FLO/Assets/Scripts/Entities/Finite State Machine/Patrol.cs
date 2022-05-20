using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public Patrol(Entity entity)
    {
        _navMeshAgent = entity.NavAgent;
        _animator = entity.Animator;
    }

    public void Tick()
    {
        PatrolArea();
    }

    public void FixedTick()
    {
        
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
    }

    private void PatrolArea()
    {
        Debug.Log("Patrolling.");
        _animator.SetBool("Running", false);
    }
}