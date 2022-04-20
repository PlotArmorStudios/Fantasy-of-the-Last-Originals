using UnityEngine;

public class Hitstun : IState
{
    private Entity _entity;
    
    public Hitstun(Entity entity)
    {
        _entity = entity;
    }

    public void Tick()
    {
        _entity.StateMachine.StunTime -= Time.deltaTime;
        if (_entity.StateMachine.StunTime <= 0)
            _entity.StateMachine.Stun = false;
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