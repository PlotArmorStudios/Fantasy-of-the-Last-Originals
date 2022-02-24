using UnityEngine;
using UnityEngine.AI;

public class ReturnHome : IState
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Vector3 _initialPosition;
    public bool IsHome { get; private set; }

    public ReturnHome(Entity entity)
    {
        _navMeshAgent = entity.NavAgent;
        _initialPosition = entity.InitialPosition;
        _animator = entity.Animator;
    }

    public void Tick()
    {
        ReturnToStartPosition();
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _animator.SetBool("Running", true);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Running", false);
    }

    void ReturnToStartPosition()
    {
        _navMeshAgent.destination = _initialPosition;
    }
}