using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        _attackTimer = 4.5f;
    }

    public void Tick()
    {
        AttackPlayer();
    }

    public void OnEnter()
    {
        _animator.SetBool("Running", false);
        _attackDelay = _entity.StateMachine.AttackDelay;
    }

    public void OnExit()
    {
        _attackTimer = 4.5f;
    }

    private void AttackPlayer()
    {
        if (_attackTimer < _attackDelay)
            _attackTimer += Time.deltaTime;

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