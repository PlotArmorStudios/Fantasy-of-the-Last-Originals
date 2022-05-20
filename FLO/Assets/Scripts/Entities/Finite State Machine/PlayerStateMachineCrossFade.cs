using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerStateMachineCrossFade : FiniteStateMachine, IStateMachine
{
    public Player Player { get; set; }
    private Animator _animator;

    private DodgeManeuver _dodge;
    private GroundCheck _groundCheck;

    private AttackingCrossFade _attackingCrossFade;
    private IdlingCrossfade _idlingCrossfade;
    private InTransitionCrossFade _inTransitionCrossFade;
    private DodgingCrossFade _dodgingCrossFade;
    private AirborneCrossFade _airborneCrossFade;
    private AirborneAttackCrossFade _airborneAttackCrossFade;
    
    protected override void Start()
    {
        Instance = this;
        Player = GetComponent<Player>();
        _animator = GetComponentInChildren<Animator>();
        _dodge = GetComponent<DodgeManeuver>();
        _stateMachine = new StateMachine();

        InitializeStates();

        AddStateTransitions();

        //Set default state
        _stateMachine.SetState(_idlingCrossfade);
    }

    protected override void InitializeStates()
    {
        _idlingCrossfade = new IdlingCrossfade(Instance);
        _attackingCrossFade = new AttackingCrossFade(Instance);
        _inTransitionCrossFade = new InTransitionCrossFade(Instance);
        _dodgingCrossFade = new DodgingCrossFade(Instance);
        _airborneCrossFade = new AirborneCrossFade(Instance);
        _airborneAttackCrossFade = new AirborneAttackCrossFade(Instance);
    }

    protected override void AddStateTransitions()
    {
        _stateMachine.AddTransition(
            _idlingCrossfade,
            _attackingCrossFade,
            () => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));

        _stateMachine.AddTransition(
            _idlingCrossfade,
            _airborneCrossFade,
            () => Player.IsJumping || Player.IsFalling);

        _stateMachine.AddTransition(
            _attackingCrossFade,
            _airborneCrossFade,
            () => Player.IsFalling);

        _stateMachine.AddTransition(
            _airborneCrossFade,
            _idlingCrossfade,
            () => Player.IsGrounded);

        _stateMachine.AddTransition(
            _airborneCrossFade,
            _airborneAttackCrossFade,
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
            _airborneAttackCrossFade,
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
            _airborneCrossFade,
            () => Player.IsFalling);

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

    protected override void Update()
    {
        _stateMachine.Tick();
    }

    protected override void FixedUpdate()
    {
        _stateMachine.FixedTick();
    }
}