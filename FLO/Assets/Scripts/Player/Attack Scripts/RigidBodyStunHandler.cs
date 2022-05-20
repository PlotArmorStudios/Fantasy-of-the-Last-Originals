//#define CalculatePhysics
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidBodyStunHandler : KnockBackHandler
{
    private bool IsAboveContactPoint;
    private float LaunchPosition { get; set; }

    public void AirLock(Transform airLockPosition)
    {
        StartCoroutine(ApplyAirLock());
    }
    public override IEnumerator ApplyAirLock()
    {
        var newPosition = Rigidbody.transform.position;
        newPosition.y += LaunchPosition;
        Rigidbody.transform.position = Vector3.Lerp(Rigidbody.transform.position, newPosition, Time.deltaTime);
        yield return null;
    }
    
    public override void ApplyGroundedAttackPull(float attackPull)
    {
        if (!GroundCheck.IsGrounded)
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, Rigidbody.velocity.y - attackPull, Rigidbody.velocity.z);
    }

    public void LimitFallAccelerationMultiplier()
    {
        if (FallAccelerationMultiplier > 5f)
            FallAccelerationMultiplier = 5f;
    }

    public override void ApplyKnockBack(Vector3 attackForce)
    {
        KnockBackForce = attackForce;
    }

    public override void ApplyHitStop(float hitStopDuration)
    {
        ApplyHitStopDuration = true;
        HitStopDuration = hitStopDuration;
    }
    
    public override void ResetDownForce()
    {
        CurrentDownForce = 0;
    }
    
    public void ApplyHitStop()
    {
        if (ApplyHitStopDuration)
            StartCoroutine(KnockBackAfterHitStop());
    }

    IEnumerator KnockBackAfterHitStop()
    {
        Rigidbody.velocity = Vector3.zero;

        yield return new WaitForSeconds(HitStopDuration);
        Rigidbody.velocity = KnockBackForce;

        ApplyHitStopDuration = false;
    }
}
