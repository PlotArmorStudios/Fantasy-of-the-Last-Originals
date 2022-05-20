using UnityEngine;

public class Idling : IState
{
    private readonly Animator _animator;
    private readonly CombatManager _combatManager;
    private readonly StanceToggler _stanceTogglerToggler;

    public Idling(Animator animator)
    {
        _animator = animator;
        _combatManager = _animator.GetComponent<CombatManager>();
        _stanceTogglerToggler = _animator.GetComponent<StanceToggler>();
        _stanceTogglerToggler.OnChangeStance += ChangeStance;
    }

    public void FixedTick()
    {
        
    }

    public void OnEnter()
    {
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Attack 2", false);
        _animator.SetBool("Attack 3", false);
        
        _combatManager.InputCount = 0;
    }

    public void Tick()
    {
        if (_combatManager.InputReceived && _combatManager.InputCount >= 1)
        {
            _animator.SetBool("Attacking", true);

            _combatManager.ReceiveInput();
            _combatManager.InputReceived = false;
        }
    }

    public void ChangeStance()
    {
        for (int i = 0; i < 4; i++)
        {
            var currentStep = i + 1;
            Debug.Log($"Deactivated Stance {currentStep}");
            _animator.SetBool($"Stance{currentStep}", false);
        }
            
        _animator.SetBool($"Stance{_stanceTogglerToggler.CurrentStance}", true);
    }
    public void OnExit()
    {
        _animator.SetBool("Attack 1", false);
    }
}