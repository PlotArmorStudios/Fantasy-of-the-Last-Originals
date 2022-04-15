using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovementOnCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private RootMotionController _rootMotion;
    
    private bool _shouldStop;
    private Collider[] _touching;
    public bool ShouldStop => _shouldStop;

    private void FixedUpdate()
    {
        _touching = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, _layerMask);
        _shouldStop = _touching.Length > 0;
        _rootMotion.StopMovement = _shouldStop;
    }
}
