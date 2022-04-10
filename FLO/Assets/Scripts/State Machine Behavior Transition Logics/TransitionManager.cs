using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TransitionManager : StateMachineBehaviour
{
    [SerializeField] private int _transitionNumber; //for example, 1
    [SerializeField] private int _transitionToAttack; //for example, 2

    private CombatManager _combatManager;
    private ToggleStance _stanceToggler;
    private PhotonView _view;

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
        if (_combatManager.InputReceived)
        {
            if (_combatManager.InputCount <= 3)
                animator.SetBool($"Attack {_combatManager.InputCount}", true);
            //Play attack 2 if attack key was hit two times (aka if hit while in this state)

            //Only transition to next attack if Attack 2 is true, you are in Transition 1 state, and current animator time is greater than value specified in TransitionSpeed variables in CombatManagerScript
            if (animator.GetBool($"Attack {_transitionToAttack}") &&
                stateInfo.IsTag("Transition"))
            {
                animator.SetBool("Attacking", true);
                animator.CrossFade($"S{_stanceToggler.CurrentStance} Attack {_transitionToAttack}", .25f, 0, 0f);

                _combatManager.ReceiveInput();
                _combatManager.InputReceived = false;
            }
        }

        if (!animator.GetBool($"Attack {_transitionToAttack}") &&
            stateInfo.IsTag("Transition") &&
            stateInfo.normalizedTime >
            _combatManager.ReturnTransitionSpeed1(_combatManager.Player.Stance))
        {
            animator.SetBool("Attacking", false);
            animator.CrossFadeInFixedTime($"Stance {_stanceToggler.CurrentStance}", .25f, 0);
        }
    }
}