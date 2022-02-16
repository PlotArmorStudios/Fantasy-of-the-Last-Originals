using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    [SerializeField] EnemyState _enemyState = EnemyState.Idle;
    [SerializeField] float _targetDetectRange = 5f;
    [SerializeField] float _attackDelay = 2f;
    [SerializeField] float _attackRange = 2f;


    public NavMeshAgent NavAgent { get; private set; }
    public Player PlayerTarget { get; private set; }
    public Animator Animator { get; private set; }
    public Vector3 InitialPosition { get; private set; }
    public float Health => _health.CurrentHealthValue;

    private RigidBodyStunHandler _rigidBodyStunHandler;
    private EnemyDeathLogic _enemyDeathLogic;
    private EnemyHealth _health;

    private float _attackTimer;
    private bool _canResetNavMesh;

    private void Awake()
    {
        PlayerTarget = FindObjectOfType<Player>();
        NavAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        InitialPosition = transform.position;

        _health = GetComponent<EnemyHealth>();
        _enemyDeathLogic = GetComponent<EnemyDeathLogic>();
        _rigidBodyStunHandler = GetComponent<RigidBodyStunHandler>();
    }

    void Update()
    {
        if (NavAgent.isActiveAndEnabled && PlayerTarget)
        {
            //SetEnemyState();
            //EnemyStateLogicHandler();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Foot") || collider.CompareTag("Hand"))
        {
            _enemyState = EnemyState.Hitstun;
        }
    }


    public void SetIdle()
    {
        _enemyState = EnemyState.Idle;
    }

    void SetEnemyState()
    {
        float enemyToPlayerDistance = Vector3.Distance(transform.position, PlayerTarget.transform.position);
        var startPosition = new Vector3(InitialPosition.x, transform.position.y, InitialPosition.z);

        if (_enemyDeathLogic.Died)
        {
            _enemyState = EnemyState.Death;
        }

        if (_enemyState != EnemyState.Death)
        {
            if (!Animator.IsInTransition(0) && Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _enemyState = EnemyState.Attack;
            }

            if (_enemyState != EnemyState.Hitstun)
            {
                if (enemyToPlayerDistance <= _targetDetectRange && enemyToPlayerDistance > _attackRange)
                    _enemyState = EnemyState.Chase;
                else if (enemyToPlayerDistance < _targetDetectRange && enemyToPlayerDistance <= _attackRange)
                    _enemyState = EnemyState.Attack;
                else if (transform.position != startPosition && enemyToPlayerDistance > _targetDetectRange)
                    _enemyState = EnemyState.Return;
                else if (transform.position == startPosition)
                    _enemyState = EnemyState.Patrol;
            }
        }
    }

    void EnemyStateLogicHandler()
    {
        if (_enemyState != EnemyState.Death)
        {
            // if (_enemyState == EnemyState.Chase)
            //     //FollowPlayer();
            //     if (_enemyState == EnemyState.Attack)
            //         //AttackPlayer();
            //         if (_enemyState == EnemyState.Patrol)
            //             //Patrol();
            //             if (_enemyState == EnemyState.Idle)
            //                 //PlayIdleAnimations();
            //                 if (_enemyState == EnemyState.Defend)
            //                     //RunDefenseAI();
            //                     if (_enemyState == EnemyState.Return)
            //                         //ReturnToStartPosition();
        }
    }

    void RunDefenseAI()
    {
        throw new NotImplementedException();
    }

    void ReturnToStartPosition()
    {
        NavAgent.isStopped = false;
        NavAgent.destination = InitialPosition;
    }

    void PlayIdleAnimations()
    {
        throw new NotImplementedException();
    }

    void Patrol()
    {
        NavAgent.isStopped = true;
        Animator.SetBool("Running", false);
    }

    void AttackPlayer()
    {
        if (_attackTimer < _attackDelay)
            _attackTimer += Time.deltaTime;

        NavAgent.isStopped = true;
        Animator.SetBool("Running", false);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(PlayerTarget.transform.position - transform.position), 5f * Time.deltaTime);

        if (_attackTimer >= _attackDelay)
        {
            Animator.SetBool("Attacking", true);
            StartCoroutine(ResetAttackTimer());
        }
        else
            Animator.SetBool("Attacking", false);
    }

    void FollowPlayer()
    {
        if (PlayerTarget)
        {
            NavAgent.isStopped = false;
            _attackTimer = 0;
            Animator.SetBool("Attacking", false);
            Animator.SetBool("Running", true);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(PlayerTarget.transform.position - transform.position), 5f * Time.deltaTime);
            NavAgent.SetDestination(PlayerTarget.transform.position);
        }
    }

    IEnumerator ResetAttackTimer()
    {
        yield return new WaitForSeconds(1f);
        _attackTimer = 0f;
    }
}