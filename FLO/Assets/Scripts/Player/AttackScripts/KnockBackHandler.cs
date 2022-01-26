using System;
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
    protected EnemyAI _enemyAI;
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
    protected bool _ApplyKnockBackForce;
    protected bool AirStall { get; set; }
    protected float _linkSkillKnockBack;
    protected bool CanResetNavAgent;
    protected bool IsRaising => _rb.velocity.y > 0;
    protected bool IsFalling => _rb.velocity.y <= 0;

    protected Vector3 _trajectory;

    public virtual void ApplyKnockBack(float attackDefinitionHitStopDuration)
    {
        throw new NotImplementedException();
    }

    public virtual void AllowKnockBackToApply(Vector3 knockBackPower)
    {
        throw new NotImplementedException();
    }

    public virtual void SetAirStall(float attackDefinitionAirStallDuration)
    {
        throw new NotImplementedException();
    }

    public virtual void SetContactPoint(SkillType attackDefinitionSkillType, Vector3 comboPoint)
    {
        throw new NotImplementedException();
    }

    public virtual void ApplyGroundedAttackPull(float attackDefinitionDownwardPull)
    {
        throw new NotImplementedException();
    }

    public virtual void ResetDownForce()
    {
        throw new NotImplementedException();
    }

    public virtual void SetDownPull(float enemyGravityResetValue)
    {
        throw new NotImplementedException();
    }
}