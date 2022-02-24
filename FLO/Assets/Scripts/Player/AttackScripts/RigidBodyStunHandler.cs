using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RigidBodyStunHandler : KnockBackHandler
{
    private bool IsAboveContactPoint;

    void Start()
    {
        _trajectory = new Vector3(0, 0, 0);
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
            if (Entity != null)
                Entity.SetIdle();
        }
    }

    void FixedUpdate()
    {
        LimitFallAccelerationMultiplier();
        ApplyHitStop();
        CalculateKnockBackPhysics();
    }

    private void CalculateKnockBackPhysics()
    {
        //Limit Down force
        if (CurrentDownForce > 3) CurrentDownForce = 3;

        if (!_groundCheck.UpdateIsGrounded())
        {

            if (_targetSkillTypeUsed == SkillType.LinkSkill)
            {
                ApplyLinkSkillGravity();
            }
            else if (_targetSkillTypeUsed != SkillType.LinkSkill)
            {
                ApplyHookSkillGravity();
            }
        }
        else
        {
            NullifyGravity();
        }

        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - CurrentDownForce, _rb.velocity.z);
    }

    private void NullifyGravity()
    {
        _fallAccelerationMultiplier = 0;
        _fallDecelerationMultiplier = 0;
        CurrentDownForce = 0;
    }

    private void ApplyHookSkillGravity()
    {
        if (IsRaising)
        {
            _fallAccelerationMultiplier += Time.deltaTime * 4f;

            CurrentDownForce = _fallAccelerationMultiplier *
                               ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);
        }

        else if (IsFalling)
        {
            _fallDecelerationMultiplier += Time.deltaTime * _weight;

            CurrentDownForce += Time.deltaTime *
                                ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                                 _weight); //down force is going to decrease over time, and decrease more over time due to fallmult

            if (CurrentDownForce > 3) CurrentDownForce = 3;

            if (CurrentDownForce < 0) CurrentDownForce = 0f;
        }
    }

    private void ApplyLinkSkillGravity()
    {
        IsAboveContactPoint = transform.position.y >= ContactPointLaunchLimiter.y;
        if (transform.position.y >= ContactPointLaunchLimiter.y)
        {
            _fallAccelerationMultiplier += Time.deltaTime * 3f;

            CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                               ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);
        }
        else if (transform.position.y < ContactPointLaunchLimiter.y)
        {
            if (IsRaising)
            {
                _fallAccelerationMultiplier += Time.deltaTime * 3f;

                CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                                   ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);
            }
            else if (IsFalling)
            {
                _fallDecelerationMultiplier = _fallAccelerationMultiplier > 2f ? 100f : 0f;
                _fallDecelerationMultiplier += Time.deltaTime * _weight;
                CurrentDownForce -=
                    Time.deltaTime *
                    ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                     _weight); //down force is going to decrease over time, and decrease more over time due to fallmult

                if (CurrentDownForce < 0) CurrentDownForce = 0f;
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
            StartCoroutine(ResetAirStall(AirStallDuration));
        }
    }

    public override void ApplyGroundedAttackPull(float attackPull)
    {
        if (!_groundCheck.IsGrounded)
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - attackPull, _rb.velocity.z);
    }


    private void ApplyHitStop()
    {
        if (ApplyHitStopDuration)
        {
            StartCoroutine(KnockBackAfterHitStop());
        }
    }

    private void LimitFallAccelerationMultiplier()
    {
        if (_fallAccelerationMultiplier > 5f)
            _fallAccelerationMultiplier = 5f;
    }

    public override void ApplyKnockBack(Vector3 attackForce)
    {
        KnockBackForce = attackForce;
        
        if (_targetSkillTypeUsed == SkillType.LinkSkill)
            _linkSkillKnockBack = attackForce.z;
    }

    public override void ApplyHitStop(float hitStopDuration)
    {
        ApplyHitStopDuration = true;
        HitStopDuration = hitStopDuration;
    }
    
    IEnumerator KnockBackAfterHitStop()
    {
        _rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(HitStopDuration);
        _rb.velocity = KnockBackForce;

        ApplyHitStopDuration = false;
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

}