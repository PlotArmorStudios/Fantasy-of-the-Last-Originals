using System;
using UnityEngine;

public class OverlapCapsuleHitBox : HitBox
{
    private Mesh _mesh;
    private GameObject _capsule;

    private void Start()
    {
        _capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        _capsule.transform.parent = transform;
        _mesh = _capsule.GetComponent<Mesh>();
    }

    protected override Collider[] OverlapPhysics()
    {
        Vector3 attackRange = new Vector3(AttackDefinition.AttackRange,
            AttackDefinition.AttackRange,
            AttackDefinition.AttackRange);
        
        return Physics.OverlapCapsule(transform.position - attackRange, transform.position + attackRange, AttackDefinition.AttackRange,
            AttackDefinition.LayerMask);
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawMesh(_mesh, transform.position, Quaternion.identity, Vector3.one);
    }
}