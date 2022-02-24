using UnityEngine;

public class Hitstun : IState
{
    public Hitstun(Entity entity)
    {
    }

    public void Tick()
    {
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    void PlayHitstunAnimation()
    {
        Debug.Log("Hitstun");
    }
}