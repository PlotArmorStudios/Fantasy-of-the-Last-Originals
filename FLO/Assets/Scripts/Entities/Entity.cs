using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Entity : Character
{
    public Rigidbody Rigidbody { get; private set; }
    public NavMeshAgent NavAgent { get; private set; }
    public Player PlayerTarget { get; private set; }
    public Animator Animator { get; private set; }
    public Vector3 InitialPosition { get; private set; }
    public EntityStateMachine StateMachine { get; private set; }
    public float Health => _health.CurrentHealthValue;
    public bool IsGrounded => _groundCheck.IsGrounded;

    private EnemyHealth _health;

    private float _attackTimer;
    private bool _canResetNavMesh;
    private GroundCheck _groundCheck;

    private void Awake()
    {
        PlayerTarget = FindObjectOfType<Player>();
        NavAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        StateMachine = GetComponent<EntityStateMachine>();
        Rigidbody = GetComponent<Rigidbody>();
        InitialPosition = transform.position;

        _groundCheck = GetComponent<GroundCheck>();
        _health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (!_groundCheck.IsGrounded)
            FallTime += Time.deltaTime;
    }
}