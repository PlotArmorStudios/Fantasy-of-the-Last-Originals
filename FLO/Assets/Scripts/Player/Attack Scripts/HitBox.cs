using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SwitchCameraOnEvent))]
public abstract class HitBox : MonoBehaviour
{
    public AttackDefinition AttackDefinition { get; set; }
    public Transform EffectPosition;

    private Rigidbody _targetRigidBody;

    //Target knock back control
    private Transform _comboGravityPoint;
    private KnockBackDecelaration _knockBackDecelarationHandler;

    private HealthLogic _targetHealthLogic;

    //Knock back metrics
    private float _defaultKnockUpStrength = 1.2f;
    private Vector3 _knockBackPower;
    private Vector3 _comboPoint;
    private int _savedTargetID;
    private int _newTargetID;

    private ImpactSoundHandler _targetSound;
    private KnockBackHandler _targetStunHandler;
    private Player _playerLogic;
    private AttackDefinitionManager _attackDefinitionManager;
    protected Vector3 _attackDirection;
    protected Collider[] _colliders;

    void OnEnable()
    {
        GetHitBoxDefinition();
        SetDefaultHitboxRange();
    }

    private void SetDefaultHitboxRange()
    {
        gameObject.transform.localScale = new Vector3(AttackDefinition.AttackRange + .1f,
            AttackDefinition.AttackRange + .1f,
            AttackDefinition.AttackRange + .1f);
    }

    private void GetHitBoxDefinition()
    {
        _attackDefinitionManager = GetComponentInParent<AttackDefinitionManager>();
        _attackDefinitionManager.ActiveHitBox = this;

        AttackDefinition = _attackDefinitionManager.CurrentAttackDefinition;
    }

    private IEnumerator DeactivateSelf()
    {
        yield return new WaitForSeconds(AttackDefinition.HitBoxLinger);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        _savedTargetID = 0;

        if (GetComponentInParent<AttackDefinitionManager>())
            GetComponentInParent<AttackDefinitionManager>().ActiveHitBox = null;

        AttackDefinition = null;
    }

    private void FixedUpdate()
    {
        DetectCollisions();
    }

    protected virtual void DetectCollisions()
    {
        _colliders =
            OverlapPhysics();

        if (_colliders.Length == 0)
            return;


        foreach (Collider collider in _colliders)
        {
            if (collider.isTrigger) return;

            _newTargetID = collider.GetInstanceID();

            //if the new enemy equals the saved enemy, return;
            if (_newTargetID == _savedTargetID) return;
            _savedTargetID = _newTargetID;

            CacheTargetComponents(collider);
            _targetStunHandler.DisableComponents();
            _targetStunHandler.Rigidbody.velocity = Vector3.zero;

            TransferInfoToTarget(collider);
            TriggerTargetEffects();
        }
    }

    private void TriggerTargetEffects()
    {
        _targetStunHandler.ParticleHitEffects.TriggerHitEffects();
    }

    public abstract Collider[] OverlapPhysics();

    protected virtual void SetAttackDirection(Collider collider)
    {
        Transform transformPointer = FindHitboxParent();

        _attackDirection = collider.gameObject.transform.position - transformPointer.position;

        Debug.DrawRay(transform.root.position, _attackDirection, Color.blue, 3);
    }

    private Transform FindHitboxParent()
    {
        Transform transformPointer = transform;
        transformPointer = GetComponentInParent<CombatManager>().transform;
        return transformPointer;
    }

    protected virtual void TransferInfoToTarget(Collider collider)
    {
        SetAttackDirection(collider);
        DoDamage();
        _targetSound.PlayHitStunSound();
        _targetStunHandler.HitStop
            .Stop(AttackDefinition.HitStopDuration, AttackDefinition.DelayBeforeHitStop);
        ChangeRigidBodySettings();
        ApplyKnockBack(_targetStunHandler);
        HandleTargetKnockback(_targetStunHandler, _attackDirection);
    }


    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        if (AttackDefinition)
            Gizmos.DrawSphere(transform.position, AttackDefinition.AttackRange);
    }

    private void HandleTargetKnockback(KnockBackHandler targetStunHandler, Vector3 attackDirection)
    {
        attackDirection.y = AttackDefinition.KnockUpStrength;
        _knockBackPower = new Vector3(attackDirection.x,
            (attackDirection.y + AttackDefinition.KnockUpStrength) * AttackDefinition.KnockUpStrength,
            attackDirection.z * AttackDefinition.KnockBackStrength);

        if (AttackDefinition.KnockUpStrength <= 0)
            targetStunHandler.StateMachine.Stun = true;
        else if (AttackDefinition.KnockUpStrength > 0)
            targetStunHandler.StateMachine.Launch = true;
    }

    private void ChangeRigidBodySettings()
    {
        _targetRigidBody.isKinematic = false;
        _targetRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                                                                            | RigidbodyConstraints.FreezeRotationZ;
    }

    private void DoDamage()
    {
        _targetHealthLogic.TakeDamage(AttackDefinition.Damage);
    }

    private void ApplyKnockBack(KnockBackHandler targetStun)
    {
        targetStun.Rigidbody.velocity = Vector3.zero;
        if (AttackDefinition.AirLock)
        {
            targetStun.AirLocked = true;
            Transform transformPointer = FindHitboxParent();
            var direction = _targetStunHandler.transform.position - transformPointer.position;
            _targetStunHandler.ApplyAirLock(transformPointer.position + direction.normalized);
            return;
        }
        else
            targetStun.AirLocked = false;

        targetStun.AirBorneKnockUp = AttackDefinition.AirBorneKnockUp;
        targetStun.StunDuration = AttackDefinition.StunDuration;
        targetStun.ApplyHitStop(AttackDefinition.HitStopDuration);
        targetStun.ApplyKnockBack(_knockBackPower);
        targetStun.ApplyGroundedAttackPull(AttackDefinition.DownwardPull);
        targetStun.ResetDownForce();
    }

    void CacheTargetComponents(Collider collider)
    {
        _targetStunHandler = collider.GetComponent<KnockBackHandler>();

        _targetRigidBody = _targetStunHandler.Rigidbody;
        _targetHealthLogic = _targetStunHandler.Health;
        _targetSound = _targetStunHandler.SoundHandler;
    }
}