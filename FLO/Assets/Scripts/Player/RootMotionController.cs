using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    private AttackPhysics _attackPhysics;
    private Vector3 _newPosition;
    private Animator _animator;
    public bool StopMovement { get; set; }
    private DodgeManeuver _dodgeManeuver;
    private void Start()
    {
        _dodgeManeuver = GetComponent<DodgeManeuver>();
        _attackPhysics = GetComponent<AttackPhysics>();
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        if (_animator && !StopMovement)
        {
            _newPosition = transform.forward * _animator.GetFloat("RootMotionZ") * Time.deltaTime;

            transform.position += _newPosition;
        }
    }

}
