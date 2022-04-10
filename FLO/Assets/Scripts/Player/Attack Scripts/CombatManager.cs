using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CombatManager : MonoBehaviour
{
    [Header("Stance 1 Transition Control")] 
    [SerializeField] private float _S1Transition1Speed = .9f;
    [SerializeField] private float _S1Transition2Speed = .9f;

    [Header("Stance 2 Transition Control")]
    [SerializeField] private float _S2Transition1Speed = .9f;
    [SerializeField] private float _S2Transition2Speed = .9f;

    [Header("Stance 3 Transition Control")]
    [SerializeField] private float _S3Transition1Speed = .9f;
    [SerializeField] private float _S3Transition2Speed = .9f;

    [Header("Stance 4 Transition Control")]
    [SerializeField] private float _S4Transition1Speed = .9f;
    [SerializeField] private float _S4Transition2Speed = .9f;
    
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

    public float ReturnTransitionSpeed1(PlayerStance stance)
    {
        float transitionSpeed = 0;

        if (stance == PlayerStance.Stance1)
        {
            transitionSpeed = _S1Transition1Speed;
        }

        if (stance == PlayerStance.Stance2)
        {
            transitionSpeed = _S2Transition1Speed;
        }

        if (stance == PlayerStance.Stance3)
        {
            transitionSpeed = _S3Transition1Speed;
        }

        if (stance == PlayerStance.Stance4)
        {
            transitionSpeed = _S4Transition1Speed;
        }

        return transitionSpeed;
    }

    public float ReturnTransitionSpeed2(PlayerStance stance)
    {
        float transitionSpeed = 0;

        if (stance == PlayerStance.Stance1)
        {
            transitionSpeed = _S1Transition2Speed;
        }

        if (stance == PlayerStance.Stance2)
        {
            transitionSpeed = _S2Transition2Speed;
        }

        if (stance == PlayerStance.Stance3)
        {
            transitionSpeed = _S3Transition2Speed;
        }

        if (stance == PlayerStance.Stance4)
        {
            transitionSpeed = _S4Transition2Speed;
        }

        return transitionSpeed;
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

    public float GetCurrentAnimatorTime(Animator targetAnim, int layer = 0)
    {
        AnimatorStateInfo animState = targetAnim.GetCurrentAnimatorStateInfo(layer);
        float currentTime = animState.normalizedTime % 1;
        return currentTime;
    }
}