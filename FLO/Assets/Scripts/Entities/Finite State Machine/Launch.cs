using System;
using UnityEngine;

public class Launch : IState
{
    private Animator _animator;
    private EntityStateMachine _stateMachine;
    private Entity _entity;
    public Launch(Entity entity)
    {
        _entity = entity;
        _animator = entity.Animator;
        _stateMachine = entity.GetComponent<EntityStateMachine>();
    }

    public void Tick()
    {
        if (_entity.IsGrounded)
            _stateMachine.Launch = false;
    }

    public void OnEnter()
    {
        _animator.CrossFade("Flyback Stun", .25f, 0);
    }

    public void OnExit()
    {
        Debug.Log("Launch");
        _animator.SetTrigger("Landing");
    }
}