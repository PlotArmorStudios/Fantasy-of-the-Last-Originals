using System;
using Photon.Pun;
using UnityEngine;

public class PlayerStateMachineCrossFade : MonoBehaviourPunCallbacks
{
    private Animator _animator;
    private StateMachine _stateMachine;
    
    private AttackingCrossFade _attackingCrossFade;
    private IdlingCrossfade _idlingCrossfade;
    private InTransitionCrossFade _inTransitionCrossFade;
    private DashingCrossFade _dashingCrossFade;
    private Dasher _dasher;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = new StateMachine();

        InitializeStates();

        AddStateTransitions();

        //Set default state
        _stateMachine.SetState(_idlingCrossfade);
    }

    private void InitializeStates()
    {
        _idlingCrossfade = new IdlingCrossfade(_animator);
        _attackingCrossFade = new AttackingCrossFade(_animator);
        _inTransitionCrossFade = new InTransitionCrossFade(_animator);
        _dashingCrossFade = new DashingCrossFade(_animator);
        _dasher = new Dasher(_animator);
    }
    
    private void AddStateTransitions()
    {
        _stateMachine.AddTransition(
            _idlingCrossfade,
            _attackingCrossFade,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));
        
        _stateMachine.AddTransition(
            _attackingCrossFade,
            _idlingCrossfade,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
        
        _stateMachine.AddTransition(
            _attackingCrossFade,
            _inTransitionCrossFade,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Transition"));
        
        _stateMachine.AddTransition(
            _inTransitionCrossFade,
            _attackingCrossFade,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));
        
        _stateMachine.AddTransition(
            _inTransitionCrossFade,
            _idlingCrossfade,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
        
        _stateMachine.AddTransition(
            _inTransitionCrossFade,
            _idlingCrossfade,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
        
        _stateMachine.AddTransition(
            _dashingCrossFade,
            _idlingCrossfade,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"));
        
        _stateMachine.AddAnyTransition(_dashingCrossFade, 
            ()=>_animator.GetCurrentAnimatorStateInfo(0).IsTag("Dashing"));
    }

    private void Update()
    {
     //   photonView.RPC("TickStateMachine", RpcTarget.All);
        _stateMachine.Tick();
    }

    [PunRPC]
    private void TickStateMachine()
    {
        _stateMachine.Tick();
    }
}