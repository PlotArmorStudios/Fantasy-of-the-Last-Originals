using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class KnockBackHandler : MonoBehaviour
{
    [Header("Gravity")] [SerializeField] public float StartDownPull = .6f;
    [SerializeField] public float DownPull = 2f;
    [SerializeField] public float Weight = .5f;
    [SerializeField] public float FallAccelerationMultiplier;
    [SerializeField] public float FallDecelerationMultiplier;
    [SerializeField] public float FallDecelerationNormalizer;
    [SerializeField] public float FallAccelerationNormalizer = .5f;

    public SkillType TargetSkillTypeUsed;

    public Rigidbody Rigidbody;

    public GroundCheck GroundCheck;

    public Entity Entity;
    public Vector3 ContactPointLaunchLimiter;
    public Vector3 KnockBackForce;
    public float StunDuration { get; set; }
    public float AttackTimer { get; set; }
    public float CurrentDownForce { get; set; }
    public float HitStopDuration { get; set; }
    public float AirStallDuration { get; set; }
    public float AirBorneKnockUp { get; set; }
    public float GroundAttackPull { get; set; }
    public bool ApplyHitStopDuration;
    public bool AirStall { get; set; }
    public bool CanResetNavAgent;
    public bool IsRaising => Rigidbody.velocity.y > 0;
    public bool IsFalling => Rigidbody.velocity.y <= 0;

    public Vector3 Trajectory;

    private void Start()
    {
        GroundCheck = GetComponent<GroundCheck>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public abstract IEnumerator ApplyAirLock();
    public abstract void ApplyHitStop(float attackDefinitionHitStopDuration);

    public abstract void ApplyKnockBack(Vector3 attackForce);

    public abstract void ApplyGroundedAttackPull(float attackDefinitionDownwardPull);

    public abstract void ResetDownForce();

    public virtual void DisableComponents()
    {
    }
}