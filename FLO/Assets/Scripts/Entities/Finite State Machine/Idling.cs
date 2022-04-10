using System;
using UnityEngine;

public class Idling : IState
{
    private readonly Animator _animator;

    public Idling(Animator animator)
    {
        _animator = animator;
    }
    private CombatManager _combatManager;

    private ToggleStance _stanceToggler;
    
    public void OnEnter()
    {
        _combatManager = _animator.GetComponent<CombatManager>();
        _stanceToggler = _animator.GetComponent<ToggleStance>();
        
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Attack 2", false);
        _animator.SetBool("Attack 3", false);
        
        _combatManager.InputCount = 0;
    }

    public void Tick()
    {
        if (_stanceToggler.StanceChanged)
        {
            _animator.CrossFade("Stance " + _stanceToggler.CurrentStance, .25f, 0);
            _stanceToggler.StanceChanged = false;
        }
        
        if (_combatManager.InputReceived && _combatManager.InputCount >= 1)
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