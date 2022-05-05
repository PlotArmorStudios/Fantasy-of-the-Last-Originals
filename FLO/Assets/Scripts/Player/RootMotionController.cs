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
    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _dodgeManeuver = GetComponent<DodgeManeuver>();
        _attackPhysics = GetComponent<AttackPhysics>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        ApplyRootMotionZ();
        ApplyRootMotionY();
        ApplyRootMotionX();
    }

    private void ApplyRootMotionX()
    {
        _newPosition = transform.right * _animator.GetFloat("RootMotionX") * Time.deltaTime;

        _rigidbody.transform.position += _newPosition;
    }

    private void ApplyRootMotionZ()
    {
        if (_animator && !StopMovement)
        {
            _newPosition = transform.forward * _animator.GetFloat("RootMotionZ") * Time.deltaTime;

            _rigidbody.transform.position += _newPosition;
        }
    }

    private void ApplyRootMotionY()
    {
        _newPosition = transform.up * _animator.GetFloat("RootMotionY") * Time.deltaTime;

        transform.position += _newPosition;
    }
}
