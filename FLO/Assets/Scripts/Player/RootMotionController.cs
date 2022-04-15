using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    private AttackPhysics _attackPhysics;
    private Vector3 _newPosition;
    public bool StopMovement { get; set; }

    private void Start()
    {
        _attackPhysics = GetComponent<AttackPhysics>();
    }

    private void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();
        if (animator && !StopMovement)
        {
            _newPosition = transform.forward * animator.GetFloat("RootMotionZ") * Time.deltaTime;

            transform.position += _newPosition;
        }
    }

}
