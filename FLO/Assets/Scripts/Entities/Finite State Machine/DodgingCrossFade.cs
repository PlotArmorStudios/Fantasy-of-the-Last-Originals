using UnityEngine;

public class DodgingCrossFade : Dasher, IState
{
    private readonly Animator _animator;

    public DodgingCrossFade(PlayerStateMachineCrossFade stateMachine) : base(stateMachine)
    {
        _animator = stateMachine.GetComponentInChildren<Animator>();
    }

    public override void Dash() => base.Dash();

    public void Tick()
    {
    }

    public void OnEnter()
    {
        _animator.GetComponent<Player>().enabled = false;
    }

    public void OnExit()
    {
        _animator.GetComponent<Player>().enabled = true;
    }
}