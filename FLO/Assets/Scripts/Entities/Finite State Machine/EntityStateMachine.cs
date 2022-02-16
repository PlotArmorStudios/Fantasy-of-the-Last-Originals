using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EntityStateMachine : MonoBehaviour
{
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

    private float _detectionRadius = 5f;
    private float _attackRadius = 2f;
    
    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _player = FindObjectOfType<Player>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        _stateMachine = new StateMachine();
        
        InitializeStates();

        AddStateTransitions();
        
        //Set default state
        _stateMachine.SetState(_idle);
    }

    private void InitializeStates()
    {
        _idle = new Idle();
        _chasePlayer = new ChasePlayer(_navMeshAgent, _player);
        _attack = new Attack();
        _dead = new Dead(_entity);
        _returnHome = new ReturnHome();
        _patrol = new Patrol();
    }

    private void AddStateTransitions()
    {
        _stateMachine.AddTransition(_idle,
            _patrol,
            () => ShouldPatrol());
        _stateMachine.AddTransition(_patrol,
            _idle,
            () => !ShouldPatrol());
        _stateMachine.AddTransition(_idle,
            _chasePlayer,
            () => Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) <
                  _detectionRadius);
        _stateMachine.AddTransition(_patrol,
            _chasePlayer,
            () => Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < _detectionRadius);
        _stateMachine.AddTransition(_chasePlayer,
            _attack,
            () => Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < _attackRadius);
        _stateMachine.AddTransition(_attack,
            _chasePlayer,
            () => Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < _detectionRadius);
        _stateMachine.AddTransition(_chasePlayer,
            _idle,
            () => Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) > _detectionRadius);
        _stateMachine.AddAnyTransition(_dead, () => _entity.Health <= 0);
    }

    private bool ShouldPatrol()
    {
        return false;
    }


    private void Update()
    {
        _stateMachine.Tick();
    }
}
