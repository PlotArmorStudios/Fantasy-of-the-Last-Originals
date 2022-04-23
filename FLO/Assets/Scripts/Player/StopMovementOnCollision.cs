using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovementOnCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private RootMotionController _rootMotion;
    private DodgeManeuver _dodgeManeuver;
    private bool _shouldStop;
    private Collider[] _touching;
    public bool ShouldStop => _shouldStop;

    private void Start()
    {
        _dodgeManeuver = GetComponentInParent<DodgeManeuver>();
    }

    private void FixedUpdate()
    {
        if (_dodgeManeuver.Dodging)
        {
            _shouldStop = false;
            _rootMotion.StopMovement = _shouldStop;
            return;
        }

        _touching = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, _layerMask);
        _shouldStop = _touching.Length > 0;
    }
}