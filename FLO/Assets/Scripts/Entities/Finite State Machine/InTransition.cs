using System;
using UnityEngine;

public class InTransition : IState
{
    private readonly Animator _animator;

    public InTransition(Animator animator)
    {
        _animator = animator;
    }
    
    public void Tick()
    {
        throw new NotImplementedException();
    }

    public void OnEnter()
    {
        throw new NotImplementedException();
    }

    public void OnExit()
    {
        throw new NotImplementedException();
    }
}