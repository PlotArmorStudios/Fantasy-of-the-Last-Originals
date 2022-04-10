using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdentity : StateMachineBehaviour
{
    [SerializeField] private int _transitionToAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CurrentAnimatorState.AttackToTransitionTo = _transitionToAttack;
    }
}