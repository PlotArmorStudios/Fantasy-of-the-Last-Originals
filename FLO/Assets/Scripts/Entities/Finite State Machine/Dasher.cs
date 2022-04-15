using UnityEngine;

public class Dasher
{
    private readonly Animator _animator;
    public Dasher(Animator animator) => _animator = animator;

    public virtual void Dash() => _animator.CrossFade("Dash", .25f);
}