using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePlayerControl : MonoBehaviour
{
    private Player _player;
    private Animator _animator;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit Stun"))
        {
            _player.enabled = false;
        }
    }
}