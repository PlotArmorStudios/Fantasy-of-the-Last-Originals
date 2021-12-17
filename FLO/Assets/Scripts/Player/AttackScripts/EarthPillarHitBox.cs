using UnityEngine;

public class EarthPillarHitBox : OverlapCapsuleHitBox
{
    private EffectAttackDefinitionManager _effectAttackDefinition;

    private void Start()
    {
        _effectAttackDefinition = GetComponentInParent<EffectAttackDefinitionManager>();
    }
    protected override void TransferInfoToTarget(Collider collider)
    {
        SetAttackDirection(collider);
        base.TransferInfoToTarget(collider);
    }

    protected override void SetAttackDirection(Collider collider)
    {
        _attackDirection = collider.transform.position - _effectAttackDefinition.AssignedPlayer.transform.position;
    }
}