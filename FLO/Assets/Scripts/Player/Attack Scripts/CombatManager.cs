using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CombatManager : MonoBehaviour
{
    public int InputCount;
    public bool CanReceiveInput { get; set; }
    public bool InputReceived { get; set; }

    protected Animator _animator;
    protected PhotonView _view;

    protected virtual void Start()
    {
        _view = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        HandleInput();
    }

    protected virtual void HandleInput()
    {
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