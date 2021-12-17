using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    private float _damagePerCollision = 10f;

    private void OnParticleCollision(GameObject collider)
    {
        var target = collider.GetComponent<EnemyAI>();
        if (target == null)
            return;
        Debug.Log("Coolided with particles");
        target.GetComponent<HealthLogic>().TakeDamage(_damagePerCollision);
    }
}
