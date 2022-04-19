using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerStateMachineCrossFade : MonoBehaviourPunCallbacks, IStateMachine
{
    private Animator _animator;
    private StateMachine _stateMachine;

    private DodgeManeuver _dodge;
    
    private AttackingCrossFade _attackingCrossFade;
    private IdlingCrossfade _idlingCrossfade;
    private InTransitionCrossFade _inTransitionCrossFade;
    private DodgingCrossFade _dodgingCrossFade;
    public bool Hitstun { get; set; }
    public bool Launch { get; set; }

    public IEnumerator SetStunFalse()
    {
        yield break;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _dodge = GetComponent<DodgeManeuver>();
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
        _dodgingCrossFade = new DodgingCrossFade(_animator);
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
            _dodgingCrossFade,
            _idlingCrossfade,
            () => _dodge.Dodging == false);
        
        _stateMachine.AddAnyTransition(_dodgingCrossFade, 
            ()=>_animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodging"));
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