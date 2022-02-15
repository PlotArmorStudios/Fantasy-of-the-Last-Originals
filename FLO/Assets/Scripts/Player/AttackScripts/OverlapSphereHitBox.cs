using System;
using System.Collections.Generic;
using UnityEngine;


public class OverlapSphereHitBox : HitBox
{
    public override Collider[] OverlapPhysics()
    {
        return Physics.OverlapSphere(transform.position, AttackDefinition.AttackRange, AttackDefinition.LayerMask);
    }
}