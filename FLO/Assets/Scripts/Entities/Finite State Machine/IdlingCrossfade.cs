using System;
using UnityEngine;

public class IdlingCrossfade : IState
{
    protected readonly Animator _animator;
    protected CombatManager _combatManager;
    protected StanceToggler _stanceToggler;
    protected FiniteStateMachine _stateMachine;
    private readonly CurrentAnimatorState _animatorState;
    private AutoTargetEnemy _autoTargeter;
    
    public IdlingCrossfade(FiniteStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _animator = stateMachine.GetComponentInChildren<Animator>();
        _stanceToggler = stateMachine.GetComponent<StanceToggler>();
        _combatManager = stateMachine.GetComponent<CombatManager>();
        _animatorState = stateMachine.GetComponent<CurrentAnimatorState>();
        _autoTargeter = stateMachine.GetComponent<AutoTargetEnemy>();
        
        _stanceToggler.OnStanceChanged += ChangeStance;
    }

    private void ChangeStance(int stanceNumber)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            _animator.CrossFade("Stance " + stanceNumber, .25f, 0, 0f, 0f);
    }

    public void OnEnter()
    {
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Attack 2", false);
        _animator.SetBool("Attack 3", false);
        _animatorState.AltitudeState = "Grounded";
        _combatManager.InputCount = 0;
    }

    public void Tick()
    {
        if (_combatManager.InputCount >= 1)
        {
            _animator.SetBool("Attacking", true);

            _animator.CrossFade($"{_animatorState.AltitudeState} S{_stanceToggler.CurrentStance} Attack 1", 0f, 0, 0f);
            //_animator.CrossFade($"S{_stanceToggler.CurrentStance} Attack 1", 0f, 0, 0f);

            _combatManager.ReceiveInput();
            _combatManager.InputReceived = false;
        }
    }

    public void OnExit()
    {
        _animator.SetBool("Attack 1", false);
    }
}