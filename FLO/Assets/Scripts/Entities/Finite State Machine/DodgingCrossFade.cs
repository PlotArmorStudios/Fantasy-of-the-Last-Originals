using UnityEngine;

public class DodgingCrossFade : Dasher, IState
{
    private readonly Animator _animator;

    public DodgingCrossFade(FiniteStateMachine stateMachine) : base(stateMachine)
    {
        _animator = stateMachine.GetComponentInChildren<Animator>();
    }

    public override void Dash() => base.Dash();

    public void Tick()
    {
    }

    public void FixedTick()
    {
        
    }

    public void OnEnter()
    {
        _animator.GetComponent<Player>().enabled = false;
        _animator.GetComponent<DodgeManeuver>().Dodging = true;
        _animator.GetComponent<DodgeManeuver>().ToggleDodge();
    }

    public void OnExit()
    {
        _animator.GetComponent<Player>().enabled = true;
    }
}