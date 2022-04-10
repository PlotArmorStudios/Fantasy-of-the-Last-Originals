using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Animator _animator;
    private StateMachine _stateMachine;
    
    private Attacking _attacking;
    private Idling _idling;
    private InTransition _inTransition;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = new StateMachine();

        InitializeStates();

        AddStateTransitions();

        //Set default state
        _stateMachine.SetState(_attacking);
    }

    private void InitializeStates()
    {
        _idling = new Idling(_animator);
        _attacking = new Attacking(_animator);
        _inTransition = new InTransition(_animator);
    }
    
    private void AddStateTransitions()
    {
        _stateMachine.AddTransition(
            _idling,
            _attacking,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));
        
        _stateMachine.AddTransition(
            _attacking,
            _idling,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
        
        _stateMachine.AddTransition(
            _attacking,
            _inTransition,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Transition"));
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}