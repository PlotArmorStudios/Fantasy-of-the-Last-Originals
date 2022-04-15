using UnityEngine;

public class InTransition : IState
{
    private readonly Animator _animator;
    private readonly CombatManager _combatManager;
    private readonly StanceToggler _stanceTogglerToggler;
    private AnimatorStateInfo _stateInfo;

    public InTransition(Animator animator)
    {
        _animator = animator;
        _stanceTogglerToggler = _animator.GetComponent<StanceToggler>();
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

        if (!_animator.GetBool($"Attack {CurrentAnimatorState.AttackToTransitionTo}") &&
            _stateInfo.normalizedTime > .9f)
        {
            _animator.SetBool("Attacking", false);
        }
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}