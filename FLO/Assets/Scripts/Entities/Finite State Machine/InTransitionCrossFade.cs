using System;
using UnityEngine;

public class InTransitionCrossFade : Dasher, IState
{
    private readonly Animator _animator;
    private readonly CombatManager _combatManager;
    private readonly StanceToggler _stanceToggler;
    private AnimatorStateInfo _stateInfo;
    private readonly CurrentAnimatorState _animatorState;
    private bool _crossFaded;

    public InTransitionCrossFade(FiniteStateMachine stateMachine) : base(stateMachine)
    {
        _animator = stateMachine.GetComponentInChildren<Animator>();
        _stanceToggler = stateMachine.GetComponent<StanceToggler>();
        _combatManager = stateMachine.GetComponent<CombatManager>();
        _animatorState = stateMachine.GetComponent<CurrentAnimatorState>();
        _stanceToggler.OnStanceChanged += ChangeStance;
    }

    private void ChangeStance(int obj)
    {
    }

    public override void Dash() => base.Dash();

    public void Tick()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_combatManager.InputReceived)
        {
            if (_combatManager.InputCount <= 3)
                _animator.SetBool($"Attack {_combatManager.InputCount}", true);

            //Play attack 2 if attack key was hit two times (aka if hit while in this state)
            //Only transition to next attack if Attack 2 is true, you are in Transition 1 state, and current animator time is greater than value specified in TransitionSpeed variables in CombatManagerScript
            if (_animator.GetBool($"Attack {_animatorState.AttackToTransitionTo}") &&
                _stateInfo.normalizedTime > .9f)
            {
                _animator.SetBool("Attacking", true);
                _animator.CrossFade(
                    $"S{_stanceToggler.CurrentStance} Attack {_animatorState.AttackToTransitionTo}", 0f, 0,
                    0f);

                _combatManager.ReceiveInput();
                _combatManager.InputReceived = false;
            }
        }

        if (!_animator.GetBool($"Attack {_animatorState.AttackToTransitionTo}") &&
            _stateInfo.normalizedTime > .9f && !_crossFaded)
        {
            _animator.SetBool("Attacking", false);
            _animator.CrossFade($"Stance {_stanceToggler.CurrentStance}", .1f, 0);
            _crossFaded = true;
        }
    }

    public void OnEnter()
    {
        _crossFaded = false;
    }

    public void OnExit()
    {
    }
}