using System;
using UnityEngine;

public class ParticleHitBox : HitBox
{
    [SerializeField] private EffectAttackDefinitionManager _effectAttackDefinition;

    private void OnParticleCollision(GameObject other)
    {
        var target = other.GetComponent<KnockBackDecelaration>();

        if (target == null) return;

        base.DetectCollisions();
    }

    public override Collider[] OverlapPhysics()
    {
        return Physics.OverlapSphere(transform.position, AttackDefinition.AttackRange, AttackDefinition.LayerMask);
    }

    protected override void SetAttackDirection(Collider collider)
    {
        _attackDirection = collider.transform.position - _effectAttackDefinition.AssignedPlayer.transform.position;
    }
}