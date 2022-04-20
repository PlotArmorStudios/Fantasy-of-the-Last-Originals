using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CombatManager : MonoBehaviour
{
    public Player Player { get; private set; }
    public int InputCount;
    public bool CanReceiveInput { get; set; }
    public bool InputReceived { get; set; }

    protected Animator _animator;
    protected PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        Player = GetComponent<Player>();
    }

    protected void Update()
    {
        HandleInput();
    }

    protected virtual void HandleInput()
    {
        if (PauseMenu.Active) return;

        if (_view.IsMine)
        {
            Attack();
            ReceiveInput();

            if (!Player.IsJumping)
            {
                if (Input.GetButtonDown("Light Attack"))
                {
                    CanReceiveInput = true;
                    InputCount++;
                }
            }
        }
    }

    protected virtual void Attack()
    {
        if (CanReceiveInput) //On button press, inputReceived turns true if canReceiveInput is true.
        {
            InputReceived = true;
            CanReceiveInput = false;
        }
        else
        {
            return;
        }
    }

    public virtual void ReceiveInput()
    {
        if (!CanReceiveInput) CanReceiveInput = true;
        else CanReceiveInput = false;
    }

    
}

public class PlayerCombatManager : CombatManager
{
    //When true, player can interrupt current attack animation. Accessed through animation events.
    public void AttackCancelPoint()
    {
        _animator.SetBool("Attacking", false);
    }

    public void AttackLockPoint()
    {
        _animator.SetBool("Attacking", true);
    }
}