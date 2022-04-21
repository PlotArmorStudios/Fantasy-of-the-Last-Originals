using UnityEngine;

public class InTransitionEntity : IState
{
    private readonly Animator _animator;
    private readonly CombatManager _combatManager;
    private readonly StanceToggler _stanceToggler;
    private AnimatorStateInfo _stateInfo;
    private readonly CurrentAnimatorState _animatorState;
    private bool _crossFaded;

    public InTransitionEntity(FiniteStateMachine stateMachine)
    {
        _animator = stateMachine.GetComponentInChildren<Animator>();
        _stanceToggler = stateMachine.GetComponent<StanceToggler>();
        _combatManager = stateMachine.GetComponent<CombatManager>();
        _animatorState = stateMachine.GetComponent<CurrentAnimatorState>();
        _stanceToggler.OnStanceChanged += ChangeStance;
    }

    private void ChangeStance(int obj)
    {
    }


    public void Tick()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (_animator.GetBool($"Attack {_animatorState.AttackToTransitionTo}") &&
            _stateInfo.normalizedTime > .9f)
        {
            _animator.SetBool("Attacking", true);
            _animator.CrossFade(
                $"S{_stanceToggler.CurrentStance} Attack {_animatorState.AttackToTransitionTo}", 0f, 0,
                0f);
        }

        if (!_animator.GetBool($"Attack {_animatorState.AttackToTransitionTo}") &&
            _stateInfo.normalizedTime > .9f && !_crossFaded)
        {
            _animator.SetBool("Attacking", false);
            _animator.CrossFade($"Stance {_stanceToggler.CurrentStance}", .1f, 0);
            _crossFaded = true;
        }
    }

    public void OnEnter()
    {
        _crossFaded = false;
    }

    public void OnExit()
    {
    }
}