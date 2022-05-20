using System;
using Photon.Pun;
using UnityEngine;

public class AttackingCrossFade : Dasher, IState
{
    private readonly Animator _animator;
    private AnimatorStateInfo _stateInfo;
    private readonly CombatManager _combatManager;
    private readonly StanceToggler _stanceToggler;
    private readonly CurrentAnimatorState _animatorState;
    private readonly int _transitionToAttack;
    private bool _crossFaded;
    public AttackingCrossFade(FiniteStateMachine stateMachine) : base(stateMachine)
    {
        _animator = stateMachine.GetComponentInChildren<Animator>();
        _combatManager = stateMachine.GetComponent<CombatManager>();
        _stanceToggler = stateMachine.GetComponent<StanceToggler>();
        _animatorState = stateMachine.GetComponent<CurrentAnimatorState>();
    }

    public void Tick()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_combatManager.InputReceived)
        {
            if (_combatManager.InputCount <= 3)
                _animator.SetBool($"Attack {_combatManager.InputCount}", true);
        }

        if (_combatManager.InputReceived)
        {
            if (Loop()) return;

            if (_animator.GetBool($"Attack {_animatorState.AttackToTransitionTo}") &&
                _stateInfo.normalizedTime > .9f)
            {
                _animator.SetBool("Attacking", true);
                _animator.CrossFade(
                    $"{_animatorState.AltitudeState} S{_stanceToggler.CurrentStance} Attack {_animatorState.AttackToTransitionTo}", 0f, 0, 0f);

                _combatManager.ReceiveInput();
                _combatManager.InputReceived = false;
            }
        }
    }

    private bool Loop()
    {
        if (_combatManager.InputCount >= 4 && _animatorState.LoopToAttack && _stateInfo.normalizedTime > .9f)
        {
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

    public virtual void OnEnter()
    {
        _crossFaded = false;
        HandleAttackScream(_animator, _stateInfo);
    }

    public virtual void OnExit()
    {
        _combatManager.InputReceived = false;
    }

    [PunRPC]
    private static void HandleAttackScream(Animator animator, AnimatorStateInfo stateInfo)
    {
        if (stateInfo.IsTag("Attack"))
        {
            var attackScreamHandler = animator.GetComponent<AttackScreamHandler>();
            var rand = UnityEngine.Random.Range(0, 3);

            if (rand == 1)
            {
                var rand2 = UnityEngine.Random.Range(0, attackScreamHandler.Clips.Length);
                attackScreamHandler.PlayScreamSound(rand2);
            }
        }
    }
}

