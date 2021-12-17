using System;
using System.Collections.Generic;
using UnityEngine;


public class OverlapSphereHitBox : HitBox
{
    protected override Collider[] OverlapPhysics()
    {
        return Physics.OverlapSphere(transform.position, AttackDefinition.AttackRange, AttackDefinition.LayerMask);
    }
}