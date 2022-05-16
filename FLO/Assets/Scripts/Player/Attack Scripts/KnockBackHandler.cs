using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class KnockBackHandler : MonoBehaviour
{
    [SerializeField] protected LayerMask _groundLayerMask;
    [SerializeField] protected Transform _feet;

    [Header("Gravity")] [SerializeField] protected float _startDownPull = .6f;
    [SerializeField] protected float _downPull = 2f;
    [SerializeField] protected float _weight = .5f;
    [SerializeField] protected float _fallAccelerationMultiplier;
    [SerializeField] protected float _fallDecelerationMultiplier;
    [SerializeField] protected float _fallDecelerationNormalizer;
    [SerializeField] protected float _fallAccelerationNormalizer = .5f;

    protected SkillType _targetSkillTypeUsed;

    protected NavMeshAgent _navMesh;
    protected Rigidbody _rb;

    public GroundCheck _groundCheck;
    public GroundCheck GroundCheck => _groundCheck;

    protected EnemyDeathLogic _enemyDeathLogic;
    protected Entity Entity;
    protected Player _playerLogic;
    protected Animator _animator;
    protected Vector3 ContactPointLaunchLimiter;
    protected Vector3 KnockBackForce;
    public float StunDuration { get; set; }
    protected float AttackTimer { get; set; }
    protected float CurrentDownForce { get; set; }
    protected float HitStopDuration { get; set; }
    protected float AirStallDuration { get; set; }
    public float AirBorneKnockUp { get; set; }
    protected float GroundAttackPull { get; set; }
    protected bool ApplyHitStopDuration;
    protected bool AirStall { get; set; }
    protected float _linkSkillKnockBack;
    protected bool CanResetNavAgent;
    protected bool IsRaising => _rb.velocity.y > 0;
    protected bool IsFalling => _rb.velocity.y <= 0;

    protected Vector3 _trajectory;

    private void Start()
    {
        _groundCheck = GetComponent<GroundCheck>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    public abstract IEnumerator ApplyAirLock();
    public abstract void ApplyHitStop(float attackDefinitionHitStopDuration);

    public abstract void ApplyKnockBack(Vector3 attackForce);

    public abstract void SetAirStall(float attackDefinitionAirStallDuration);

    public abstract void SetContactPoint(SkillType attackDefinitionSkillType, Vector3 comboPoint);

    public abstract void ApplyGroundedAttackPull(float attackDefinitionDownwardPull);

    public abstract void ResetDownForce();

    public abstract void SetDownPull(float enemyGravityResetValue);

    public virtual void DisableComponents()
    {
    }
}