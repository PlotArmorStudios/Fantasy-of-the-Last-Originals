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

    private Animator _animator;
    private PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        Player = GetComponent<Player>();
    }

    void Update()
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

    public void Attack()
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

    public void ReceiveInput()
    {
        if (!CanReceiveInput) CanReceiveInput = true;
        else CanReceiveInput = false;
    }

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