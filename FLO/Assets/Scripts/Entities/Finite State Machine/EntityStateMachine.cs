//#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EntityStateMachine : FiniteStateMachine, IStateMachine
{
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private float _guardRadius = 2f;
    [SerializeField] private float _attackDelay = 2f;
    [SerializeField] private float _returnHomeTime = 4f;
    [SerializeField] private float _homeRadius = 2f;

    public StateMachine StateMachine;
    public NavMeshAgent NavMeshAgent;
    public Player Player;
    public Entity Entity;
    public Animator Animator;

    public Idle _idle;
    public ChasePlayer _chasePlayer;
    public Attack _attack;
    public Dead _dead;
    public ReturnHome _returnHome;
    public Patrol _patrol;
    public Launch _launch;
    public Hitstun _hitstun;
    public OnGuard _onGuard;

    public float AttackDelay => _attackDelay;
    public float ReturnHomeTime => _returnHomeTime;
    private bool IsHome => Vector3.Distance(Entity.transform.position, Entity.InitialPosition) <= _homeRadius;
    public float DistanceToPlayer => Vector3.Distance(NavMeshAgent.transform.position, Player.transform.position);
    public bool Invulnerable => false;
    public float GuardRadius => _guardRadius;
    
    public bool AttackPhase { get; set; }

    public float StunTime = .5f;
    private InTransitionEntity _inTransition;

    public bool Land { get; set; }
    public float FallTime => Entity.FallTime;

    private void Start()
    {
        Instance = this;
        Animator = GetComponent<Animator>();
        Entity = GetComponent<Entity>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Player = FindObjectOfType<Player>();

        StateMachine = new StateMachine();

        InitializeStates();

        AddStateTransitions();

        //Set default state
        StateMachine.SetState(_idle);
    }

    public bool CanChase { get; set; }

    protected override void InitializeStates()
    {
        _idle = new Idle(Instance);
        _chasePlayer = new ChasePlayer(Entity, Player, NavMeshAgent);
        _attack = new Attack(Instance);
        _inTransition = new InTransitionEntity(Instance);
        _dead = new Dead(Entity);
        _returnHome = new ReturnHome(Entity);
        _patrol = new Patrol(Entity);
        _launch = new Launch(Entity);
        _hitstun = new Hitstun(Entity);
        _onGuard = new OnGuard(Instance);
    }

    protected override void AddStateTransitions()
    {
        StateMachine.AddTransition(
            _idle,
            _patrol,
            () => ShouldPatrol());

        StateMachine.AddTransition(
            _patrol,
            _idle,
            () => !ShouldPatrol());

        StateMachine.AddTransition(
            _idle,
            _chasePlayer,
            () => DistanceToPlayer < _detectionRadius && CanChase);

        StateMachine.AddTransition(
            _patrol,
            _chasePlayer,
            () => DistanceToPlayer < _detectionRadius);

        StateMachine.AddTransition(
            _chasePlayer,
            _onGuard,
            () => DistanceToPlayer < _guardRadius);
        
        StateMachine.AddTransition(
            _onGuard,
            _chasePlayer,
            () => DistanceToPlayer > _guardRadius + 1);
        
        StateMachine.AddTransition(
            _onGuard,
            _attack,
            () => AttackPhase);
        
        StateMachine.AddTransition(
            _attack,
            _onGuard,
            () => !AttackPhase);

        StateMachine.AddTransition(
            _inTransition,
            _attack,
            () => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));
        
        StateMachine.AddTransition(
            _inTransition,
            _onGuard,
            () => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") || !AttackPhase);

        StateMachine.AddTransition(
            _attack,
            _inTransition,
            () => Animator.GetCurrentAnimatorStateInfo(0).IsTag("Transition"));

        StateMachine.AddTransition(
            _chasePlayer,
            _idle,
            () => DistanceToPlayer > _detectionRadius * 3);

        StateMachine.AddTransition(
            _idle,
            _returnHome,
            () => !IsHome && _idle.UpdateReturnHomeTime());

        StateMachine.AddTransition(
            _returnHome,
            _idle,
            () => IsHome);

        StateMachine.AddTransition(
            _returnHome,
            _chasePlayer,
            () => DistanceToPlayer < _detectionRadius);
        StateMachine.AddTransition(
            _launch,
            _idle,
            () => FallTime > 0 && Entity.IsGrounded);
        StateMachine.AddTransition(
            _hitstun,
            _idle,
            () => !Stun);

        StateMachine.AddAnyTransition(_dead, () => Entity.Health <= 0);
        StateMachine.AddAnyTransition(_launch, () => !Invulnerable && Launch);
        StateMachine.AddAnyTransition(_hitstun, () => !Invulnerable && Stun);
    }


    public void InjectCoroutine(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
        StartCoroutine(coroutine);
    }

    public IEnumerator ToggleStun()
    {
        Stun = true;
        yield return new WaitForSeconds(.5f);
        Stun = false;
    }

    public IEnumerator ToggleLaunch()
    {
        Launch = true;
        yield return new WaitForSeconds(.5f);
        Launch = false;
    }

    private bool ShouldPatrol()
    {
        return false;
    }

    protected override void Update()
    {
#if DEBUG_LOG
        Debug.Log("Launch: " + Launch);
        Debug.Log("Hitstun: " + Hitstun);
        Debug.Log(FallTime);
#endif
        StateMachine.Tick();
    }
}