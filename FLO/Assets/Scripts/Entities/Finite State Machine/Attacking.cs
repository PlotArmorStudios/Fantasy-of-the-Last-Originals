using System;
using UnityEngine;

public class Attacking : IState
{
    private readonly Animator _animator;
    private AnimatorStateInfo _stateInfo;
    private CombatManager _combatManager;
    private ToggleStance _stanceToggler;
    private int _transitionToAttack;

    public Attacking(Animator animator)
    {
        _animator = animator;
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _stanceToggler = _animator.GetComponent<ToggleStance>();
    }
    
    public void Tick()
    {
        if (_combatManager.InputReceived)
        {
            if (_combatManager.InputCount <= 3)
                _animator.SetBool($"Attack {_combatManager.InputCount}", true);
            //Play attack 2 if attack key was hit two times (aka if hit while in this state)

            //Only transition to next attack if Attack 2 is true, you are in Transition 1 state, and current animator time is greater than value specified in TransitionSpeed variables in CombatManagerScript
            if (_animator.GetBool($"Attack {_transitionToAttack}") &&
                _stateInfo.IsTag("Transition"))
            {
                _animator.SetBool("Attacking", true);
                _animator.CrossFade($"S{_stanceToggler.CurrentStance} Attack {_transitionToAttack}", .25f, 0, 0f);

                _combatManager.ReceiveInput();
                _combatManager.InputReceived = false;
            }
        }

        if (!_animator.GetBool($"Attack {_transitionToAttack}") &&
            _stateInfo.IsTag("Transition") &&
            _stateInfo.normalizedTime >
            _combatManager.ReturnTransitionSpeed1(_combatManager.Player.Stance))
        {
            _animator.SetBool("Attacking", false);
            _animator.CrossFadeInFixedTime($"Stance {_stanceToggler.CurrentStance}", .25f, 0);
        }
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