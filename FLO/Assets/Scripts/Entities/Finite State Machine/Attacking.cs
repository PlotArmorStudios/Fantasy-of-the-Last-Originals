using System;
using Photon.Pun;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class Attacking : IState
{
    private readonly Animator _animator;
    private AnimatorStateInfo _stateInfo;
    private CombatManager _combatManager;
    private ToggleStance _stanceToggler;
    private int _transitionToAttack;

    public Attacking(Animator animator)
    {
        _animator = animator;
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _stanceToggler = _animator.GetComponent<ToggleStance>();
        _combatManager = _animator.GetComponent<CombatManager>();
    }

    public void Tick()
    {
        if (_combatManager.InputReceived)
        {
            if (_combatManager.InputCount <= 3)
                _animator.SetBool($"Attack {_combatManager.InputCount}", true);
        }
    }

    public void OnEnter()
    {
        HandleAttackScream(_animator, _stateInfo);
    }

    public void OnExit()
    {
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