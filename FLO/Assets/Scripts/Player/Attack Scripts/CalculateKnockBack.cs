//#define RunLinkSkillCode

using System.Collections;
using System.Collections.Generic;
using EntityStates;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CalculateKnockBack : MonoBehaviour
{
    private RigidBodyStunHandler _stunHandler;

    void Start()
    {
        _stunHandler = GetComponent<RigidBodyStunHandler>();
    }

    void Update()
    
    
    {
        if (_stunHandler.GroundCheck.UpdateIsGrounded())
            _stunHandler.DownPull = _stunHandler.StartDownPull;
    }

    void FixedUpdate()
    {
        _stunHandler.LimitFallAccelerationMultiplier();
        _stunHandler.ApplyHitStop();
        CalculateKnockBackPhysics();
    }

    public void CalculateKnockBackPhysics()
    {
        //Limit Down force
        if (_stunHandler.CurrentDownForce > 3) _stunHandler.CurrentDownForce = 3;
        if (_stunHandler.CurrentDownForce < 0) _stunHandler.CurrentDownForce = 0f;

        if (!_stunHandler.GroundCheck.UpdateIsGrounded())
        {
#if RunLinkSkillCode
            if (_stunHandler.TargetSkillTypeUsed == SkillType.LinkSkill)
                ApplyLinkSkillGravity();
#endif
            ApplyHookSkillGravity();
        }
        else
        {
            NullifyGravity();
        }

        _stunHandler.Rigidbody.velocity = new Vector3(_stunHandler.Rigidbody.velocity.x,
            _stunHandler.Rigidbody.velocity.y - _stunHandler.CurrentDownForce, _stunHandler.Rigidbody.velocity.z);
    }

    private void NullifyGravity()
    {
        _stunHandler.FallAccelerationMultiplier = 0;
        _stunHandler.FallDecelerationMultiplier = 0;
        _stunHandler.CurrentDownForce = 0;
    }

    private void ApplyHookSkillGravity()
    {
        if (_stunHandler.IsRaising)
        {
            _stunHandler.FallAccelerationMultiplier += Time.fixedDeltaTime * 4f;

            _stunHandler.CurrentDownForce = _stunHandler.FallAccelerationMultiplier *
                                            ((_stunHandler.FallAccelerationMultiplier *
                                              _stunHandler.FallAccelerationNormalizer) * _stunHandler.Weight);
        }
        else if (_stunHandler.IsFalling)
        {
            _stunHandler.FallDecelerationMultiplier += Time.fixedDeltaTime * _stunHandler.Weight;

            _stunHandler.CurrentDownForce = .1f;
        }
    }

    #region StashedLinkSkillCode

    /// <summary>
    ///CAN BE REMOVED
    /// </summary>
#if RunLinkSkillCode
    private void ApplyLinkSkillGravity()
    {
        IsAboveContactPoint = _stunHandler.Rigidbody.transform.position.y >= _stunHandler.ContactPointLaunchLimiter.y;

        if (_stunHandler.Rigidbody.transform.position.y >= _stunHandler.ContactPointLaunchLimiter.y)
        {
            _stunHandler.FallAccelerationMultiplier += Time.fixedDeltaTime * 3f;

            _stunHandler.CurrentDownForce = _stunHandler.DownPull * _stunHandler.FallAccelerationMultiplier *
                                            ((_stunHandler.FallAccelerationMultiplier * _stunHandler.FallAccelerationNormalizer) * _stunHandler.Weight);
        }
        else if (_stunHandler.Rigidbody.transform.position.y < _stunHandler.ContactPointLaunchLimiter.y)
        {
            if (_stunHandler.IsRaising)
            {
                _stunHandler.FallAccelerationMultiplier += Time.fixedDeltaTime * 3f;

                _stunHandler.CurrentDownForce = _stunHandler.DownPull * _stunHandler.FallAccelerationMultiplier *
                                                ((_stunHandler.FallAccelerationMultiplier * _stunHandler.FallAccelerationNormalizer) * _stunHandler.Weight);
            }
            else if (_stunHandler.IsFalling)
            {
                _stunHandler.FallDecelerationMultiplier = _stunHandler.FallAccelerationMultiplier > 2f ? 100f : 0f;
                _stunHandler.FallDecelerationMultiplier += Time.fixedDeltaTime * _stunHandler.Weight;
                _stunHandler.CurrentDownForce -=
                    Time.fixedDeltaTime *
                    ((_stunHandler.FallDecelerationMultiplier * _stunHandler.FallDecelerationNormalizer) *
                     _stunHandler.Weight); //down force is going to decrease over time, and decrease more over time due to fallmult

                if (_stunHandler.CurrentDownForce < 0)_stunHandler.CurrentDownForce = 0f;
            }
        }
    }
#endif

    #endregion
}