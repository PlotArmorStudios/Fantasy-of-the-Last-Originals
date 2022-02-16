using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EntityStateMachine : MonoBehaviour
{
    private StateMachine _stateMachine;
    private NavMeshAgent _navMeshAgent;
    private Player _player;
    private Entity _entity;

    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _player = FindObjectOfType<Player>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        _stateMachine = new StateMachine();
        
        var idle = new Idle();
        var chasePlayer = new ChasePlayer(_navMeshAgent, _player);
        var attack = new Attack();
        var dead = new Dead(_entity);
        var returnHome = new ReturnHome();
        
        _stateMachine.AddTransition(idle, 
            chasePlayer,
            ()=> Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < 5f); //placeholder condition
        _stateMachine.AddTransition(chasePlayer, 
            attack,
            ()=> Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < 2f);
        _stateMachine.AddAnyTransition(dead, () => _entity.Health <= 0);
        _stateMachine.SetState(idle);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}
