using System;
using UnityEngine;
using UnityEngine.AI;

public class EntityStateMachine : MonoBehaviour
{
    private StateMachine _stateMachine;
    private NavMeshAgent _navMeshAgent;
    private Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        _stateMachine = new StateMachine();
        
        var idle = new Idle();
        var chasePlayer = new ChasePlayer(_navMeshAgent);
        var attack = new Attack();
        
        _stateMachine.Add(idle);
        _stateMachine.Add(chasePlayer);
        _stateMachine.Add(attack);
        
        _stateMachine.AddTransition(idle, 
            chasePlayer,
            ()=> Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < 5f); //placeholder condition
        _stateMachine.AddTransition(chasePlayer, 
            attack,
            ()=> Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) < 2f);
        
        _stateMachine.SetState(idle);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}