using UnityEngine;

public class Dasher
{
    private readonly Animator _animator;
    public Dasher(FiniteStateMachine stateMachine) => _animator = stateMachine.GetComponentInChildren<Animator>();

    public virtual void Dash() => _animator.CrossFade("Dash", .25f);
}