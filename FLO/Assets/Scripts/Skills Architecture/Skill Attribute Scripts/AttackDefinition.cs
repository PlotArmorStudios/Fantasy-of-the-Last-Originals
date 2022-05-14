using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Definition")]
public class AttackDefinition : ScriptableObject
{
    public LayerMask LayerMask;
    public SkillType SkillType = SkillType.LinkSkill;
    public StunType StunType = StunType.HitStun;

    public float Damage = 10f;
    public float AttackRange = 1f;
    public float KnockBackStrength = 6f;
    public float KnockUpStrength = 2f;
    public float StunDuration = .1f;
    
    public float AirBorneKnockUp = 1;
    public float LaunchLimiter = 1f;

    public float DecelerationDuration = 5f;
    public float HitStopDuration = .2f;
    public float DelayBeforeHitStop = .2f;
    public float AirStallDuration = .8f;
    public float DownwardPull;

    public float HitBoxLinger = .2f;
    
    [Header("Effects")]
    public GameObject Effect;

    public float EffectPositionX;
    public float EffectPositionY;
    public float EffectPositionZ;
    
    public float EffectRotationX;
    public float EffectRotationY;
    public float EffectRotationZ;

    public float EffectScaleX;
    public float EffectScaleY;
    public float EffectScaleZ;
}