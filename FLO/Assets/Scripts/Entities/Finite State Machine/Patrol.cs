using UnityEngine;

public class Patrol : IState
{
    public void Tick()
    {
        Debug.Log("Patrolling.");
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}