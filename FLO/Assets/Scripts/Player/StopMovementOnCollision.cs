using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovementOnCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private bool _shouldStop;
    private Collider[] _touching;
    public bool ShouldStop => _shouldStop;

    private void FixedUpdate()
    {
        _touching = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, _layerMask);
        _shouldStop = _touching.Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.localScale / 4);
    }
}
