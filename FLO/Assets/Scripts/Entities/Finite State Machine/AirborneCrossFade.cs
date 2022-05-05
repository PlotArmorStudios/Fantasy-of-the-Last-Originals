using UnityEngine;

public class AirborneCrossFade : IState
{
    private PlayerStateMachineCrossFade _stateMachine;
    private readonly Animator _animator;
    private readonly StanceToggler _stanceToggler;
    private readonly CombatManager _combatManager;
    private readonly CurrentAnimatorState _animatorState;

    public AirborneCrossFade(FiniteStateMachine stateMachine)
    {
        _stateMachine = (PlayerStateMachineCrossFade) stateMachine;
        _animator = stateMachine.GetComponentInChildren<Animator>();
        _stanceToggler = stateMachine.GetComponent<StanceToggler>();
        _combatManager = stateMachine.GetComponent<CombatManager>();
        _animatorState = stateMachine.GetComponent<CurrentAnimatorState>();
    }
    
    public void Tick()
    {
        if (_combatManager.InputCount >= 1)
        {
            _animator.SetBool("Attacking", true);

            _animator.CrossFade($"{_animatorState.AltitudeState} S1 Attack 1", 0f, 0, 0f);

            _combatManager.ReceiveInput();
            _combatManager.InputReceived = false;
        }
    }

    public void OnEnter()
    {
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Attack 2", false);
        _animator.SetBool("Attack 3", false);
        _animatorState.AltitudeState = "Airborne";
        _combatManager.InputCount = 0;
    }

    public void OnExit()
    {
        _animator.SetBool("Attack 1", false);
    }
}