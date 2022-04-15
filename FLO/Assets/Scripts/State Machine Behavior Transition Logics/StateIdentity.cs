using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdentity : StateMachineBehaviour
{
    [SerializeField] private int _transitionToAttack;
    [SerializeField] private bool _loopToAttack;
    [SerializeField] private int _attackToLoopTo;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CurrentAnimatorState.AttackToTransitionTo = _transitionToAttack;
        CurrentAnimatorState.LoopToAttack = _loopToAttack;
        CurrentAnimatorState.AttackToLoopTo = _attackToLoopTo;
    }
}