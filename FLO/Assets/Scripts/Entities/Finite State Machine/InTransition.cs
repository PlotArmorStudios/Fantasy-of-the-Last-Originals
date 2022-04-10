using System;
using UnityEngine;

public class InTransition : IState
{
    private readonly Animator _animator;
    private AnimatorStateInfo _stateInfo;
    private CombatManager _combatManager;
    private ToggleStance _stanceToggler;

    public InTransition(Animator animator)
    {
        _animator = animator;
        _stanceToggler = _animator.GetComponent<ToggleStance>();
        _combatManager = _animator.GetComponent<CombatManager>();
    }

    public void Tick()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_combatManager.InputReceived)
        {
            if (_combatManager.InputCount <= 3)
                _animator.SetBool($"Attack {_combatManager.InputCount}", true);
            //Play attack 2 if attack key was hit two times (aka if hit while in this state)

            //Only transition to next attack if Attack 2 is true, you are in Transition 1 state, and current animator time is greater than value specified in TransitionSpeed variables in CombatManagerScript
            if (_animator.GetBool($"Attack {CurrentAnimatorState.AttackToTransitionTo}"))
            {
                _animator.SetBool("Attacking", true);
                _animator.CrossFade(
                    $"S{_stanceToggler.CurrentStance} Attack {CurrentAnimatorState.AttackToTransitionTo}", .25f, 0, 0f);

                _combatManager.ReceiveInput();
                _combatManager.InputReceived = false;
            }
        }

        if (!_animator.GetBool($"Attack {CurrentAnimatorState.AttackToTransitionTo}") &&
            _stateInfo.normalizedTime > .9f)
        {
            _animator.SetBool("Attacking", false);
            _animator.CrossFade($"Stance {_stanceToggler.CurrentStance}", .25f, 0);
        }
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}