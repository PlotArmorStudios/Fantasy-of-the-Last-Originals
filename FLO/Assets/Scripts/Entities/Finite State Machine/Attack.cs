using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Attack : IState
{
    private readonly Entity _entity;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly Player _player;
    private EntityStateMachine _stateMachine;
    private float _attackTimer;
    private float _attackDelay;
    private EntityCombatManager _combatManager;
    private AnimatorStateInfo _stateInfo;
    private CurrentAnimatorState _animatorState;
    private EntityStanceToggler _stanceToggler;
    private bool _crossFaded;

    public Attack(FiniteStateMachine stateMachine)
    {
        _stateMachine = stateMachine as EntityStateMachine;
        _combatManager = (EntityCombatManager) _stateMachine.GetComponent<CombatManager>();
        _animatorState = _stateMachine.GetComponent<CurrentAnimatorState>();
        _stanceToggler = (EntityStanceToggler) _stateMachine.GetComponent<StanceToggler>();
        _entity = _stateMachine.Entity;
        _player = _stateMachine.Player;
        _animator = _entity.Animator;
        _attackTimer = 4.5f;
    }

    public void Tick()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        FocusPlayer();

        if (_combatManager.InputReceived)
        {
            if (Loop()) return;

            TriggerAttack();
        }
    }

    public void TriggerAttack()
    {
        if (_animator.GetBool($"Attack {_animatorState.AttackToTransitionTo}") &&
            _stateInfo.normalizedTime > .9f)
        {
            _animator.SetBool("Attacking", true);
            _animator.CrossFade(
                $"S{_stanceToggler.CurrentStance} Attack {_animatorState.AttackToTransitionTo}", 0f, 0, 0f);

            _combatManager.ReceiveInput();
            _combatManager.InputReceived = false;
        }
    }

    private bool Loop()
    {
        if (_combatManager.InputCount >= 4 && _animatorState.LoopToAttack && _stateInfo.normalizedTime > .9f)
        {
            Debug.Log("Looooop");
            _animator.CrossFade(
                $"S{_stanceToggler.CurrentStance} Attack {_animatorState.AttackToLoopTo}", 0f, 0, 0f);

            _animator.SetBool("Attacking", false);
            _animator.SetBool("Attack 2", false);
            _animator.SetBool("Attack 3", false);

            _combatManager.InputCount = 0;
            _combatManager.InputReceived = false;
            return true;
        }

        return false;
    }

    public void OnEnter()
    {
        _animator.SetBool("Running", false);
        _crossFaded = false;
        _attackDelay = _entity.StateMachine.AttackDelay;
    }

    public void OnExit()
    {
        _attackTimer = 4.5f;
        _combatManager.InputReceived = false;
    }

    private void FocusPlayer()
    {
        _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation,
            Quaternion.LookRotation(_player.transform.position - _entity.transform.position), 5f * Time.deltaTime);
    }
}