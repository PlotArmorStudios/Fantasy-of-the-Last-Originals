using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPlatformLogic : MonoBehaviour
{
    [SerializeField] private Transform _rootTransform;
    [SerializeField] private LayerMask _layerMask;

    private void FixedUpdate()
    {
        var touchingPlayer =
            Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, _layerMask);
        
        foreach (Collider collider in touchingPlayer)
        {
            Debug.Log("Moving enemy back");
            _rootTransform.position += Vector3.back;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}