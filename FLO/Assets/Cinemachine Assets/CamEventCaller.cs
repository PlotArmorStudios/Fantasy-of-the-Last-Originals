using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CamEventCaller : MonoBehaviour
{
    private HitBox _referencedHitbox;
    private Collider[] _colliders;

    private void OnEnable()
    {
        _referencedHitbox = GetComponent<HitBox>();
    }

    private void FixedUpdate()
    {
        _colliders =
            _referencedHitbox.OverlapPhysics();

        if (_colliders.Length == 0)
            return;

        var target = _colliders[0];
        
        Debug.Log(target);
        
        if (target == null)
            return;
        
        Debug.Log("Switch Cams");
        if (CameraSwitcher.TurnOnTargetCam == false)
        {
            CameraSwitcher.TurnOnTargetCam = true;
        }
    }
}

public abstract class State
{
    public IEnumerator Idle()
    {
        yield break;
    } 
    public IEnumerator Attack()
    {
        yield break;
    }
    
    public IEnumerator Heal()
    {
        yield break;
    }

    public IEnumerator Pause()
    {
        yield break;
    }

    public IEnumerator Resume()
    {
        yield break;
    }

    public IEnumerator Hitstun()
    {
        yield break;
    }
}