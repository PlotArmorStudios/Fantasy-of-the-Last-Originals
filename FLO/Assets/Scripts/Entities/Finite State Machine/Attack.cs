using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Attack : IState
{
    private readonly Entity _entity;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly Player _player;

    private float _attackTimer;
    private float _attackDelay;

    public Attack(Entity entity, Player player)
    {
        _entity = entity;
        _player = player;
        
        _animator = _entity.Animator;
    }

    public void Tick()
    {
        AttackPlayer();
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    void AttackPlayer()
    {
        if (_attackTimer < _attackDelay)
            _attackTimer += Time.deltaTime;

        _animator.SetBool("Running", false);
        _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation,
            Quaternion.LookRotation(_player.transform.position - _entity.transform.position), 5f * Time.deltaTime);

        if (_attackTimer >= _attackDelay)
        {
            _animator.SetBool("Attacking", true);
            _attackTimer = 0;
        }
        else
        {
            _animator.SetBool("Attacking", false);
        }
    }
}