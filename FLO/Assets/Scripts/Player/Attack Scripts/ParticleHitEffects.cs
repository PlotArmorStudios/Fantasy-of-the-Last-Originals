using System.Collections.Generic;
using UnityEngine;

public class ParticleHitEffects : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _hitEffects;

    public void TriggerHitEffects()
    {
        var randomEffectNumber = Random.Range(0, _hitEffects.Count - 1);
        
        if (_hitEffects.Count > 0)
            _hitEffects[randomEffectNumber].Play();
    }
}