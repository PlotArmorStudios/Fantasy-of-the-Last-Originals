using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerStateMachineCrossFade : MonoBehaviourPunCallbacks, IStateMachine
{
    private Animator _animator;
    private StateMachine _stateMachine;
    private PlayerStateMachineCrossFade Instance;

    private DodgeManeuver _dodge;

    private AttackingCrossFade _attackingCrossFade;
    private IdlingCrossfade _idlingCrossfade;
    private InTransitionCrossFade _inTransitionCrossFade;
    private DodgingCrossFade _dodgingCrossFade;
    private StanceToggler _stanceToggler;
    public bool Stun { get; set; }
    public bool Launch { get; set; }


    private void Start()
    {
        Instance = this;
        _stanceToggler = GetComponent<StanceToggler>();
        _animator = GetComponentInChildren<Animator>();
        _dodge = GetComponent<DodgeManeuver>();
        _stateMachine = new StateMachine();

        InitializeStates();

        AddStateTransitions();

        //Set default state
        _stateMachine.SetState(_idlingCrossfade);
    }

    private void InitializeStates()
    {
        _idlingCrossfade = new IdlingCrossfade(Instance);
        _attackingCrossFade = new AttackingCrossFade(Instance);
        _inTransitionCrossFade = new InTransitionCrossFade(Instance);
        _dodgingCrossFade = new DodgingCrossFade(Instance);
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
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodging"));
    }

    public IEnumerator ToggleStun()
    {
        yield break;
    }

    public IEnumerator ToggleLaunch()
    {
        yield break;
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