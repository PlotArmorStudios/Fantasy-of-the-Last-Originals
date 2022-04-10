using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private CombatManager _combatManager;

    private ToggleStance _stanceToggler;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combatManager = animator.GetComponent<CombatManager>();
        _stanceToggler = animator.GetComponent<ToggleStance>();
        
        animator.SetBool("Attacking", false);
        animator.SetBool("Attack 2", false);
        animator.SetBool("Attack 3", false);
        
        _combatManager.InputCount = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_stanceToggler.StanceChanged)
        {
            animator.CrossFade("Stance " + _stanceToggler.CurrentStance, .25f, 0);
            _stanceToggler.StanceChanged = false;
        }
        
        if (_combatManager.InputReceived && _combatManager.InputCount >= 1)
        {
            animator.SetBool("Attacking", true);

            animator.CrossFade($"S{_stanceToggler.CurrentStance} Attack 1", 0f, 0, 0f);
            
            _combatManager.ReceiveInput();
            _combatManager.InputReceived = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack 1", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
