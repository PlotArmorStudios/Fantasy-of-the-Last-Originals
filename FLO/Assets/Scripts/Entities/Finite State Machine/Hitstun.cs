using UnityEngine;

public class Hitstun : IState
{
    private Entity _entity;
    private readonly RigidBodyStunHandler _stunHandler;
    private Animator _animator;

    public Hitstun(Entity entity)
    {
        _entity = entity;
        _stunHandler = entity.GetComponent<RigidBodyStunHandler>();
        _animator = entity.GetComponent<Animator>();
    }

    public void Tick()
    {
        _entity.StateMachine.StunTime -= Time.deltaTime;
        if (_entity.StateMachine.StunTime <= 0)
            _entity.StateMachine.Stun = false;
    }

    public void FixedTick()
    {
        // _stunHandler.LimitFallAccelerationMultiplier();
        // _stunHandler.ApplyHitStop();
    }

    public void OnEnter()
    {
        _animator.CrossFade("Hit Stun", .25f, 0);
    }

    public void OnExit()
    {
    }
}