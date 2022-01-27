using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RigidBodyStunHandler : KnockBackHandler
{
    // Start is called before the first frame update
    void Start()
    {
        _trajectory = new Vector3(0, 0, 0);
        _groundCheck = GetComponent<GroundCheck>();
        _navMesh = GetComponent<NavMeshAgent>();
        _enemyAI = GetComponent<EnemyAI>();
        _playerLogic = GetComponent<Player>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnValidate()
    {
        if (!GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (_groundCheck.UpdateIsGrounded())
        {
            _downPull = _startDownPull;
            if (_enemyAI != null)
                _enemyAI.SetIdle();
        }
    }
    
    void FixedUpdate()
    {
        LimitFallAccelerationMultiplier();
        ApplyKnockBackForce();

        if (!_groundCheck.UpdateIsGrounded())
        {
            ApplyAirStall();

            if (_targetSkillTypeUsed == SkillType.LinkSkill)
            {
                ApplyLinkSkillForces();
            }
            else if (_targetSkillTypeUsed != SkillType.LinkSkill)
            {
                ApplyHookSkillForces();
            }
        }
        else
        {
            _fallAccelerationMultiplier = 0;
            _fallDecelerationMultiplier = 0;
            CurrentDownForce = 0;
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }
    }

    private void ApplyHookSkillForces()
    {
        if (IsRaising)
        {
            _fallAccelerationMultiplier += Time.deltaTime * 4f;

            CurrentDownForce = _fallAccelerationMultiplier *
                               ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }

        else if (IsFalling)
        {
            _fallDecelerationMultiplier += Time.deltaTime * _weight;

            CurrentDownForce += Time.deltaTime *
                                ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                                 _weight); //down force is going to decrease over time, and decrease more over time due to fallmult
            
            if (CurrentDownForce > 8)
                CurrentDownForce = 8;

            if (CurrentDownForce < 0)
                CurrentDownForce = 0f;
            
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }
    }


    private void ApplyLinkSkillForces()
    {
        if (transform.position.y >= ContactPointLaunchLimiter.y)
        {
            _fallAccelerationMultiplier += Time.deltaTime * 3f;

            CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                               ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

            if (_rb.velocity.z > 0)
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            if (_rb.velocity.z < 0)
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
        }
        else if (transform.position.y < ContactPointLaunchLimiter.y)
        {
            if (IsRaising)
            {
                _fallAccelerationMultiplier += Time.deltaTime;

                CurrentDownForce = _fallAccelerationMultiplier *
                                   ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);

                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            }
            else if (IsFalling)
            {
                if (_fallAccelerationMultiplier > 2f)
                {
                    _fallDecelerationMultiplier = 100f;
                }
                else
                {
                    _fallDecelerationMultiplier = 0f;
                }

                _fallDecelerationMultiplier += Time.deltaTime * _weight;
                CurrentDownForce -=
                    Time.deltaTime *
                    ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                     _weight); //down force is going to decrease over time, and decrease more over time due to fallmult
                
                if (CurrentDownForce < 0)
                    CurrentDownForce = 0f;

                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            }
        }
    }

    private void ApplyAirStall()
    {
        if (AirStall)
        {
            _fallAccelerationMultiplier = 0;
            _fallDecelerationMultiplier = 0;
            CurrentDownForce = 0;
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
            StartCoroutine(ResetAirStall(AirStallDuration));
        }
    }

    public override void ApplyGroundedAttackPull(float attackPull)
    {
        if (!_groundCheck.IsGrounded)
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - attackPull, _rb.velocity.z);
    }


    private void ApplyKnockBackForce()
    {
        if (_ApplyKnockBackForce)
        {
            StartCoroutine(KnockBackAfterHitStop(KnockBackForce));
        }
    }

    public void DisableNavMesh()
    {
        if (_navMesh)
            _navMesh.enabled = false;
    }

    private void LimitFallAccelerationMultiplier()
    {
        if (_fallAccelerationMultiplier > 5f)
            _fallAccelerationMultiplier = 5f;
    }

    public override void AllowKnockBackToApply(Vector3 attackForce)
    {
        KnockBackForce = attackForce;
        if (_targetSkillTypeUsed == SkillType.LinkSkill)
        {
            _linkSkillKnockBack = attackForce.z;
        }
    }

    public override void ApplyKnockBack(float hitStopDuration)
    {
        _ApplyKnockBackForce = true;
        HitStopDuration = hitStopDuration;
    }

    public override void SetContactPoint(SkillType skillType, Vector3 contactPoint)
    {
        _targetSkillTypeUsed = skillType;
        ContactPointLaunchLimiter = contactPoint;
    }

    public override void SetAirStall(float airStallDuration)
    {
        AirStall = true;
        AirStallDuration = airStallDuration;
    }

    public override void ResetDownForce()
    {
        CurrentDownForce = 0;
    }

    public override void SetDownPull(float downPull)
    {
        _downPull = downPull;
    }

    IEnumerator ResetAirStall(float airStallDuration)
    {
        yield return new WaitForSeconds(airStallDuration);
        AirStall = false;
    }

    IEnumerator KnockBackAfterHitStop(Vector3 knockBack)
    {
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(HitStopDuration);
        _rb.velocity = KnockBackForce;

        _ApplyKnockBackForce = false;
    }
}