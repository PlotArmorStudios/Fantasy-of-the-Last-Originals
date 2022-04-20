using UnityEngine;

public class ToggleEntity : MonoBehaviour
{
    private EntityStateMachine _stateMachine;

    private void Start() => _stateMachine = GetComponent<EntityStateMachine>();

    public void ToggleEntityOff()
    {
        
    }

    public void ToggleEntityOn()
    {
    }
}