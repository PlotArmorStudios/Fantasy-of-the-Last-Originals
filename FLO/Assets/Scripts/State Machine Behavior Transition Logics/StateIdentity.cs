using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdentity : StateMachineBehaviour
{
    [SerializeField] private int _transitionToAttack;
    [SerializeField] private bool _loopToAttack;
    [SerializeField] private int _attackToLoopTo;

    private CurrentAnimatorState _animatorState;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animatorState = animator.GetComponent<CurrentAnimatorState>();
        _animatorState.AttackToTransitionTo = _transitionToAttack;
        _animatorState.LoopToAttack = _loopToAttack;
        _animatorState.AttackToLoopTo = _attackToLoopTo;
    }
}