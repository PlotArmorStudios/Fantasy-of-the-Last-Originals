using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class S1Transition1Behaviour : StateMachineBehaviour
{
    private CombatManager _combatManager;
    private PhotonView _view;
    private ToggleStance _stanceToggler;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combatManager = animator.GetComponent<CombatManager>();
        _view = animator.GetComponent<PhotonView>();
        _stanceToggler = animator.GetComponent<ToggleStance>();

        HandleAttackScream(animator, stateInfo);
    }
    
    [PunRPC]
    private static void HandleAttackScream(Animator animator, AnimatorStateInfo stateInfo)
    {
        if (stateInfo.IsTag("Attack"))
        {
            var attackScreamHandler = animator.GetComponent<AttackScreamHandler>();
            var rand = Random.Range(0, 3);

            if (rand == 1)
            {
                var rand2 = Random.Range(0, attackScreamHandler.Clips.Length);
                attackScreamHandler.PlayScreamSound(rand2);
            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Play attack 3 as well if attack key was hit three times
        if (_combatManager.InputCount == 3)
        {
            animator.SetBool("Attack 3", true);
        }

        if (_combatManager.InputReceived && _combatManager.InputCount >= 2)
        {
            //Play attack 2 if attack key was hit two times (aka if hit while in this state)
            animator.SetBool("Attack 2", true);

            //Only transition to next attack if Attack 2 is true, you are in Transition 1 state, and current animator time is greater than value specified in TransitionSpeed variables in CombatManagerScript
            if (animator.GetBool("Attack 2") && stateInfo.IsName("S1 Transition 1"))
            {
                animator.SetBool("Attacking", false);

                if (_stanceToggler.Stance == PlayerStance.Stance1)
                {
                    animator.SetBool("Attacking", true);
                    animator.CrossFade("S1 Attack 2", .25f, 0, 0f);
                }
                if (_stanceToggler.Stance == PlayerStance.Stance1)
                {
                    animator.SetBool("Attacking", true);
                    animator.CrossFade("S2 Attack 2", .25f, 0, 0f);
                }
                if (_stanceToggler.Stance == PlayerStance.Stance1)
                {
                    animator.SetBool("Attacking", true);
                    animator.CrossFade("S3 Attack 2", .25f, 0, 0f);
                }
                if (_stanceToggler.Stance == PlayerStance.Stance1)
                {
                    animator.SetBool("Attacking", true);
                    animator.CrossFade("S4 Attack 2", .25f, 0, 0f);
                }

                _combatManager.ReceiveInput();
                _combatManager.InputReceived = false;
            }
        }

        if (!animator.GetBool("Attack 2") && stateInfo.IsName("S1 Transition 1") && stateInfo.normalizedTime > _combatManager.ReturnTransitionSpeed1(_combatManager.Player.Stance))
        {
            animator.SetBool("Attacking", false);
            if (_stanceToggler.Stance == PlayerStance.Stance1) animator.CrossFadeInFixedTime("Stance 1", .25f, 0);
            if (_stanceToggler.Stance == PlayerStance.Stance2) animator.CrossFadeInFixedTime("Stance 2", .25f, 0);
            if (_stanceToggler.Stance == PlayerStance.Stance3) animator.CrossFadeInFixedTime("Stance 3", .25f, 0);
            if (_stanceToggler.Stance == PlayerStance.Stance4) animator.CrossFadeInFixedTime("Stance 4", .25f, 0);
        }

        //*USE THIS TO IMPLEMENT SMOOTH TRANSITIONS FROM STANCE TO STANCE WHILE NOT USING MECANIM TRANSITIONS
        //else if(!animator.GetBool("Attack 2") && stateInfo.IsTag("S1 Transition 1") && CombatManagerScript.instance.GetCurrentAnimatorTime(animator, 0) > .9)
        //{
        //    animator.CrossFade("Stance 1 Blend Tree", 0f, 0);
        //}
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
