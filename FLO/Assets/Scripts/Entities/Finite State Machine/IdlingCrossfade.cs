using System;
using UnityEngine;

public class IdlingCrossfade : Dasher, IState
{
    private readonly Animator _animator;
    private readonly CombatManager _combatManager;
    private readonly StanceToggler _stanceToggler;

    public IdlingCrossfade(PlayerStateMachineCrossFade stateMachine) : base(stateMachine)
    {
        _animator = stateMachine.GetComponentInChildren<Animator>();
        _stanceToggler = stateMachine.GetComponent<StanceToggler>();
        _combatManager = stateMachine.GetComponent<CombatManager>();
        _stanceToggler.OnStanceChanged += ChangeStance;
    }

    private void ChangeStance(int stance)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            _animator.CrossFade("Stance " + stance, .25f, 0, 0f, 0f);
    }

    public override void Dash() => base.Dash();

    public void OnEnter()
    {
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Attack 2", false);
        _animator.SetBool("Attack 3", false);

        _combatManager.InputCount = 0;
    }

    public void Tick()
    {
        if (_combatManager.InputCount >= 1)
        {
            _animator.SetBool("Attacking", true);

            _animator.CrossFade($"S{_stanceToggler.CurrentStance} Attack 1", 0f, 0, 0f);

            _combatManager.ReceiveInput();
            _combatManager.InputReceived = false;
        }
    }

    public void OnExit()
    {
        _animator.SetBool("Attack 1", false);
    }
}