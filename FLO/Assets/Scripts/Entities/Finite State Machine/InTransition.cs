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
    }

    public void FixedTick()
    {
        
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}