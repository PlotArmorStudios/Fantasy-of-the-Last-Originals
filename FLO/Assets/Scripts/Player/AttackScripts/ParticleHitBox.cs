using System;
using UnityEngine;

public class ParticleHitBox : HitBox
{
    private EffectAttackDefinitionManager _effectAttackDefinition;

    private void Start()
    {
        _effectAttackDefinition = GetComponentInParent<EffectAttackDefinitionManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        var target = other.GetComponent<KnockBackDecelaration>();

        if (target == null) return;

        base.DetectCollisions();
    }

    protected override Collider[] OverlapPhysics()
    {
        return Physics.OverlapSphere(transform.position, AttackDefinition.AttackRange, AttackDefinition.LayerMask);
        ;
    }

    protected override void SetAttackDirection(Collider collider)
    {
        _attackDirection = collider.transform.position - _effectAttackDefinition.AssignedPlayer.transform.position;
    }

    protected override void OnDrawGizmosSelected()
    {
    }
}