using System;
using UnityEngine;

public class IdlingCrossfade : IState
{
    protected readonly Animator _animator;
    protected CombatManager _combatManager;
    protected StanceToggler _stanceToggler;
    protected FiniteStateMachine _stateMachine;

    public IdlingCrossfade(FiniteStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
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

    public void OnEnter()
    {
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Attack 2", false);
        _animator.SetBool("Attack 3", false);

        _combatManager.InputCount = 0;
        //_stateMachine.GetComponent<TogglePlayer>().TogglePlayerOn();
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