using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private CombatManager _combatManager;

    private StanceToggler _stanceTogglerToggler;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combatManager = animator.GetComponent<CombatManager>();
        _stanceTogglerToggler = animator.GetComponent<StanceToggler>();
        
        animator.SetBool("Attacking", false);
        animator.SetBool("Attack 2", false);
        animator.SetBool("Attack 3", false);
        
        _combatManager.InputCount = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_stanceTogglerToggler.StanceChanged)
        {
            animator.CrossFade("Stance " + _stanceTogglerToggler.CurrentStance, .25f, 0);
            _stanceTogglerToggler.StanceChanged = false;
        }
        
        if (_combatManager.InputReceived && _combatManager.InputCount >= 1)
        {
            animator.SetBool("Attacking", true);

            animator.CrossFade($"S{_stanceTogglerToggler.CurrentStance} Attack 1", 0f, 0, 0f);
            
            _combatManager.ReceiveInput();
            _combatManager.InputReceived = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack 1", false);
    }
}
