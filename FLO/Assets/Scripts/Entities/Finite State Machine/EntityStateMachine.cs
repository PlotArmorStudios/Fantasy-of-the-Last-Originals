//#define DEBUG_LOG

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EntityStateMachine : FiniteStateMachine, IStateMachine
{
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private float _attackRadius = 2f;
    [SerializeField] private float _attackDelay = 2f;
    [SerializeField] private float _returnHomeTime = 4f;
    [SerializeField] private float _homeRadius = 2f;

    private StateMachine _stateMachine;
    private NavMeshAgent _navMeshAgent;
    private Player _player;
    private Entity _entity;

    private Idle _idle;
    private ChasePlayer _chasePlayer;
    private Attack _attack;
    private Dead _dead;
    private ReturnHome _returnHome;
    private Patrol _patrol;
    private Launch _launch;
    private Hitstun _hitstun;

    public float AttackDelay => _attackDelay;
    public float ReturnHomeTime => _returnHomeTime;
    private bool IsHome => Vector3.Distance(_entity.transform.position, _entity.InitialPosition) <= _homeRadius;
    private float DistanceToPlayer => Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position);
    public bool Invulnerable => false;

    public float StunTime = .5f;

    public bool Land { get; set; }
    public float FallTime => _entity.FallTime;

    private void Start()
    {
        _entity = GetComponent<Entity>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = FindObjectOfType<Player>();

        _stateMachine = new StateMachine();

        InitializeStates();

        AddStateTransitions();

        //Set default state
        _stateMachine.SetState(_idle);
    }

    public bool CanChase { get; set; }

    protected override void InitializeStates()
    {
        _idle = new Idle(_entity);
        _chasePlayer = new ChasePlayer(_entity, _player, _navMeshAgent);
        _attack = new Attack(_entity, _player);
        _dead = new Dead(_entity);
        _returnHome = new ReturnHome(_entity);
        _patrol = new Patrol(_entity);
        _launch = new Launch(_entity);
        _hitstun = new Hitstun(_entity);
    }

    protected override void AddStateTransitions()
    {
        _stateMachine.AddTransition(
            _idle,
            _patrol,
            () => ShouldPatrol());

        _stateMachine.AddTransition(
            _patrol,
            _idle,
            () => !ShouldPatrol());

        _stateMachine.AddTransition(
            _idle,
            _chasePlayer,
            () => DistanceToPlayer < _detectionRadius && CanChase);

        _stateMachine.AddTransition(
            _patrol,
            _chasePlayer,
            () => DistanceToPlayer < _detectionRadius);

        _stateMachine.AddTransition(
            _chasePlayer,
            _attack,
            () => DistanceToPlayer < _attackRadius);

        _stateMachine.AddTransition(
            _attack,
            _chasePlayer,
            () => DistanceToPlayer > _attackRadius);
        _stateMachine.AddTransition(
            _attack,
            _idle,
            () => DistanceToPlayer > _attackRadius);
        _stateMachine.AddTransition(
            _chasePlayer,
            _idle,
            () => DistanceToPlayer > _detectionRadius);

        _stateMachine.AddTransition(
            _idle,
            _returnHome,
            () => !IsHome && _idle.UpdateReturnHomeTime());

        _stateMachine.AddTransition(
            _returnHome,
            _idle,
            () => IsHome);

        _stateMachine.AddTransition(
            _returnHome,
            _chasePlayer,
            () => DistanceToPlayer < _detectionRadius);
        _stateMachine.AddTransition(
            _launch,
            _idle,
            () => FallTime > 0 && _entity.IsGrounded);
        _stateMachine.AddTransition(
            _hitstun,
            _idle,
            () => !Stun);

        _stateMachine.AddAnyTransition(_dead, () => _entity.Health <= 0);
        _stateMachine.AddAnyTransition(_launch, () => !Invulnerable && Launch);
        _stateMachine.AddAnyTransition(_hitstun, () => !Invulnerable && Stun);
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
        _stateMachine.Tick();
    }
}