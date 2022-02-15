using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Attacking,
    HitStun,
    KnockBack,
    Dead
}
public class PlayerStateControl : MonoBehaviour
{
    public PlayerState PlayerState;
    private PlayerState _currentState;
    
    private Player _player;
    private RigidBodyStunHandler _rigidBodyStunHandler;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }

    void Update()
    {
        PlayerStateLogicHandler();
    }
    public void SetPlayerState(PlayerState state)
    {
        if (_currentState == state)
            return;
        
        _currentState = state;
        switch (state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Attacking:
                break;
            case PlayerState.Running:
                break;
            case PlayerState.HitStun:
                break;
            case PlayerState.KnockBack:
                break;
            case PlayerState.Dead:
                break;
        }
    }

    void PlayerStateLogicHandler()
    {
        if (PlayerState != PlayerState.Dead)
        {
            if (PlayerState == PlayerState.Idle)
            {
            }
            if (PlayerState == PlayerState.Running)
            {
            }
            if (PlayerState == PlayerState.Attacking)
            {
            }
            if (PlayerState == PlayerState.HitStun)
                TogglePlayerControl(false);
            if (PlayerState == PlayerState.KnockBack)
                TogglePlayerControl(false);
        }
    }

    void TogglePlayerControl(bool toggle)
    {
        _player.enabled = toggle;
    }
}
