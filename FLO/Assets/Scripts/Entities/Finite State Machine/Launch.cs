using System;
using UnityEngine;

public class Launch : IState
{
    private Animator _animator;
    public Launch(Entity entity)
    {
        _animator = entity.Animator;
    }

    public void Tick()
    {
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