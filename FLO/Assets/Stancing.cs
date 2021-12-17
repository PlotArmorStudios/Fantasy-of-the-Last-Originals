using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stancing : MonoBehaviour
{
    private Animator _animator;
    private float _nextTriggerTime;
    private float _triggerTImer = 15f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _nextTriggerTime += Time.time;
    }

    private void Update()
    {
        if (Time.time > _nextTriggerTime)
        {
            _animator.SetTrigger("Stancing");
            _nextTriggerTime = Time.time + _triggerTImer;
        }
    }
}
