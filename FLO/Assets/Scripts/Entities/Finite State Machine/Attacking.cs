using Photon.Pun;
using UnityEngine;

public class Attacking : IState
{
    private readonly Animator _animator;
    private readonly CombatManager _combatManager;
    private readonly int _transitionToAttack;
    private AnimatorStateInfo _stateInfo;

    public Attacking(Animator animator)
    {
        _animator = animator;
        _combatManager = _animator.GetComponent<CombatManager>();
    }

    public void Tick()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (_combatManager.InputReceived)
        {
            if (_combatManager.InputCount <= 3)
                _animator.SetBool($"Attack {_combatManager.InputCount}", true);
        }
    }

    public void FixedTick()
    {
        throw new System.NotImplementedException();
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