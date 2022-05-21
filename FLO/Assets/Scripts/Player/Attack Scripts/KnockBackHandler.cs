using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class KnockBackHandler : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] public float StartDownPull = .6f;
    [SerializeField] public float DownPull = 2f;
    [SerializeField] public float Weight = .5f;
    [SerializeField] public float FallAccelerationMultiplier;
    [SerializeField] public float FallDecelerationMultiplier;
    [SerializeField] public float FallDecelerationNormalizer;
    [SerializeField] public float FallAccelerationNormalizer = .5f;

    public Rigidbody Rigidbody;

    public GroundCheck GroundCheck;

    public Vector3 KnockBackForce;
    public float StunDuration { get; set; }
    public float CurrentDownForce { get; set; }
    public float HitStopDuration { get; set; }
    public float AirBorneKnockUp { get; set; }
    public float GroundAttackPull { get; set; }
    public bool ApplyHitStopDuration;
    public bool IsRaising => Rigidbody.velocity.y > 0;
    public bool IsFalling => Rigidbody.velocity.y <= 0;

    public HealthLogic Health { get; set; }
    public ImpactSoundHandler SoundHandler { get; set; }
    public HitStop HitStop { get; set; }
    public IStateMachine StateMachine { get; set; }
    public ParticleHitEffects ParticleHitEffects { get; set; }
    [field: SerializeField] public bool AirLocked { get; set; }

    protected virtual void Start()
    {
        GroundCheck = GetComponent<GroundCheck>();
        Rigidbody = GetComponent<Rigidbody>();
        Health = GetComponent<HealthLogic>();
        SoundHandler = GetComponent<ImpactSoundHandler>();
        HitStop = GetComponent<HitStop>();
        StateMachine = GetComponent<IStateMachine>();
        ParticleHitEffects = GetComponent<ParticleHitEffects>();
    }

    public abstract IEnumerator ApplyAirLock(Vector3 position);
    public abstract void ApplyHitStop(float attackDefinitionHitStopDuration);

    public abstract void ApplyKnockBack(Vector3 attackForce);

    public abstract void ApplyGroundedAttackPull(float attackDefinitionDownwardPull);

    public abstract void ResetDownForce();

    public virtual void DisableComponents()
    {
    }
}