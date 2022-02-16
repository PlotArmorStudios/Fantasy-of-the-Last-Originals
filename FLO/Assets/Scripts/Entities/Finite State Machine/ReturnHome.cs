using UnityEngine;
using UnityEngine.AI;

public class ReturnHome : IState
{
    private NavMeshAgent _navMeshAgent;
    private Vector3 _initialPosition;

    public ReturnHome(Entity entity)
    {
        _navMeshAgent = entity.NavAgent;
        _initialPosition = entity.InitialPosition;
    }
    
    public void Tick()
    {
        ReturnToStartPosition();
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        Debug.Log("Exit return home");
    }
    
    void ReturnToStartPosition()
    {
        _navMeshAgent.destination = _initialPosition;
    }
}