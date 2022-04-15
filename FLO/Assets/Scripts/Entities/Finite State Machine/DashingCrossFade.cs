using UnityEngine;

public class DashingCrossFade : Dasher, IState
{
    private readonly Animator _animator;

    public DashingCrossFade(Animator animator) : base(animator)
    {
        _animator = animator;
    }

    public override void Dash() => base.Dash();

    public void Tick()
    {
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}