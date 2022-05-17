using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        DisableMeshRendererIfPresent();
        AddSwitchCameraComponent();
        FindComboGravityPoint();
        ActivateComboGravityPointIfLinkSkill();

        SetDefaultHitboxRange();

        if (!GetComponent<TriggerStunAnimation>())
            AddProperHitstunComponent();
    }

    private void SetDefaultHitboxRange()
    {
        gameObject.transform.localScale = new Vector3(AttackDefinition.AttackRange + .1f,
            AttackDefinition.AttackRange + .1f,
            AttackDefinition.AttackRange + .1f);
    }

    private void AddProperHitstunComponent()
    {
        if (AttackDefinition.StunType == StunType.HitStun)
            gameObject.AddComponent<TriggerHitStunAnimation>();

        if (AttackDefinition.StunType == StunType.KnockBack)
            gameObject.AddComponent<TriggerKnockBackAnimation>();
    }

    private void ActivateComboGravityPointIfLinkSkill()
    {
        if (AttackDefinition.SkillType == SkillType.LinkSkill && _comboGravityPoint != null)
            _comboGravityPoint.gameObject.SetActive(true);
    }

    private void FindComboGravityPoint()
    {
        _comboGravityPoint = GetComponentInParent<CombatManager>().ComboGravityPoint;
        Debug.Log("Hit enemy 9.5 grab combo gravity point");
    }

    private void AddSwitchCameraComponent()
    {
        if (!GetComponent<SwitchCameraOnEvent>())
            gameObject.AddComponent<SwitchCameraOnEvent>();
    }

    private void DisableMeshRendererIfPresent()
    {
        if (GetComponent<MeshRenderer>())
            GetComponent<MeshRenderer>().enabled = false;
    }

    private void SetTriggerCollider()
    {
        if (GetComponent<Collider>())
            GetComponent<Collider>().isTrigger = true;
        else
        {
            gameObject.AddComponent<Collider>();
            GetComponent<Collider>().isTrigger = true;
        }
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
        if (GetComponent<TriggerStunAnimation>())
            RemoveStunTypeComponent();

        _savedTargetID = 0;

        if (_comboGravityPoint != null)
            if (_comboGravityPoint.gameObject.activeInHierarchy)
                _comboGravityPoint.gameObject.SetActive(false);

        if (GetComponentInParent<AttackDefinitionManager>())
            GetComponentInParent<AttackDefinitionManager>().ActiveHitBox = null;

        AttackDefinition = null;
    }

    private void RemoveStunTypeComponent()
    {
        Destroy(GetComponent<TriggerStunAnimation>());
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

            Debug.Log("Hit enemy 1");
            GetComponent<TriggerStunAnimation>().TriggerAnimation(collider);
            CacheTargetComponents(collider);
            _targetStunHandler.DisableComponents();
            TransferInfoToTarget(collider);
            TriggerTargetEffects();
            Debug.Log("Hit enemy 3");
        }
    }

    private void TriggerTargetEffects()
    {
        Debug.Log("Hit enemy 5 trigger effects");

        _targetStunHandler.GetComponent<ParticleHitEffects>().TriggerHitEffects();
    }

    public abstract Collider[] OverlapPhysics();

    protected virtual void SetAttackDirection(Collider collider)
    {
        Transform transformPointer = FindHitboxParent();

        Debug.Log("Hit enemy 7 before set direction");
        _attackDirection = collider.gameObject.transform.position - transformPointer.position;
        Debug.Log("Hit enemy 8 after set direction");

        Debug.DrawRay(transform.root.position, _attackDirection, Color.blue, 3);
    }

    private Transform FindHitboxParent()
    {
        Transform transformPointer = transform;
        transformPointer = GetComponentInParent<CombatManager>().transform;
        Debug.Log("Hit enemy 6 return parent direction");
        return transformPointer;
    }

    protected virtual void TransferInfoToTarget(Collider collider)
    {
        Debug.Log("Hit enemy 4 info transfer");

        SetAttackDirection(collider);

        Debug.Log("Hit enemy 9");
        CheckSkillType();

        Debug.Log("Hit enemy 15 narrowed ");
        DoDamage();

        Debug.Log("Hit enemy 10");
        _targetSound.PlayHitStunSound();
        _targetStunHandler.GetComponent<HitStop>()
            .Stop(AttackDefinition.HitStopDuration, AttackDefinition.DelayBeforeHitStop);

        Debug.Log("Hit enemy 11");
        ChangeRigidBodySettings();

        Debug.Log("Hit enemy 12");
        HandleTargetKnockback(_targetStunHandler, _attackDirection);
        Debug.Log("Hit enemy 13");

        ApplyKnockBack(_targetStunHandler);
        Debug.Log("Hit enemy 14");
        ApplyKnockBackDeceleration();
        Debug.Log("Hit enemy 15");
    }


    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        if (AttackDefinition)
            Gizmos.DrawSphere(transform.position, AttackDefinition.AttackRange);
    }

    private void HandleTargetKnockback(KnockBackHandler targetStunHandler, Vector3 attackDirection)
    {
        if (AttackDefinition.KnockUpStrength <= 0)
            StartCoroutine(targetStunHandler.GetComponent<IStateMachine>().ToggleStun());
        else if (AttackDefinition.KnockUpStrength > 0)
            StartCoroutine(targetStunHandler.GetComponent<IStateMachine>().ToggleLaunch());

        if (targetStunHandler.GroundCheck.UpdateIsGrounded())
        {
            attackDirection.y = AttackDefinition.KnockUpStrength;
            _knockBackPower = new Vector3(attackDirection.x,
                (attackDirection.y + AttackDefinition.KnockUpStrength) * AttackDefinition.KnockUpStrength,
                attackDirection.z * AttackDefinition.KnockBackStrength);
        }
        else
        {
            attackDirection.y = 0;
            _knockBackPower = new Vector3(attackDirection.x, attackDirection.y + AttackDefinition.AirBorneKnockUp,
                attackDirection.z * AttackDefinition.KnockBackStrength);
        }
    }


    private void ChangeRigidBodySettings()
    {
        _targetRigidBody.isKinematic = false;
        _targetRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                                                                            | RigidbodyConstraints.FreezeRotationZ;
    }

    private void CheckSkillType()
    {
        if (AttackDefinition.SkillType == SkillType.LinkSkill)
            _comboPoint = new Vector3(_comboGravityPoint.transform.position.x,
                _comboGravityPoint.transform.position.y + AttackDefinition.LaunchLimiter,
                _comboGravityPoint.transform.position.z);
    }

    private void DoDamage()
    {
        _targetHealthLogic.TakeDamage(AttackDefinition.Damage);
    }

    private void ApplyKnockBack(KnockBackHandler targetStun)
    {
        targetStun.AirBorneKnockUp = AttackDefinition.AirBorneKnockUp;
        targetStun.StunDuration = AttackDefinition.StunDuration;
        targetStun.ApplyHitStop(AttackDefinition.HitStopDuration);
        targetStun.ApplyKnockBack(_knockBackPower);
        targetStun.SetContactPoint(AttackDefinition.SkillType, _comboPoint);
        targetStun.ApplyGroundedAttackPull(AttackDefinition.DownwardPull);
        targetStun.ResetDownForce();
    }

    void ApplyKnockBackDeceleration()
    {
        if (_knockBackDecelarationHandler != null)
        {
            _knockBackDecelarationHandler.SetKnockBackTrue(true);
            _knockBackDecelarationHandler.SetKnockBackDeceleration(AttackDefinition.KnockBackStrength);
            _knockBackDecelarationHandler.SetDecelerationDuration(AttackDefinition.DecelerationDuration);
        }
    }

    void CacheTargetComponents(Collider collider)
    {
        Debug.Log("Hit enemy 2");

        _targetStunHandler = collider.GetComponent<KnockBackHandler>();
        _knockBackDecelarationHandler = collider.GetComponent<KnockBackDecelaration>();
        _targetRigidBody = collider.GetComponent<Rigidbody>();
        _targetHealthLogic = collider.GetComponent<HealthLogic>();
        _targetSound = collider.GetComponent<ImpactSoundHandler>();
    }
}