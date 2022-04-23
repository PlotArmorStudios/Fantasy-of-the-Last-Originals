using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeManeuver : MonoBehaviour
{
    [SerializeField] String _playerMask;
    [SerializeField] String _enemyMask;
    [SerializeField] float _dashForce = 50f;
    [SerializeField] float _dashDuration = 2f;

    Rigidbody _rb;
    Animator _animator;

    private bool _dashing;
    private int _enemyMaskInt;
    private int _playerMaskInt;

    public bool Dodging { get; set; }

    void Start()
    {
        _enemyMaskInt = LayerMask.NameToLayer(_enemyMask);
        _playerMaskInt = LayerMask.NameToLayer(_playerMask);
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dodge());
            _animator.SetBool("Attacking", false);
        }
    }

    public void ToggleDodge()
    {
        StartCoroutine(SetDodgeTrue());
    }

    public void ToggleDodge(int dodging)
    {
        Dodging = dodging == 1;
        Debug.Log("Dodging is: " + Dodging);
    }

    IEnumerator SetDodgeTrue()
    {
        yield return new WaitForSeconds(_dashDuration);
        Dodging = false;
    }


    IEnumerator Dodge()
    {
        _animator.SetTrigger("Dodge");

        PassThroughEnemies(true);
        yield return new WaitForSeconds(_dashDuration);
        PassThroughEnemies(false);
    }

    private void PassThroughEnemies(bool pass)
    {
        Physics.IgnoreLayerCollision(_playerMaskInt, _enemyMaskInt, pass);
    }
}