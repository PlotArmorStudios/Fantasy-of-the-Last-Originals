using UnityEngine;

public class ToggleEntity : MonoBehaviour
{
    private EntityStateMachine _stateMachine;

    private void Start() => _stateMachine = GetComponent<EntityStateMachine>();

    public void ToggleEntityOff() => _stateMachine.enabled = false;

    public void ToggleEntityOn() => _stateMachine.enabled = true;
}