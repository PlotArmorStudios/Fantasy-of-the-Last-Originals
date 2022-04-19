using UnityEngine;

public class ToggleBehaviors : MonoBehaviour
{
    private Behaviour[] _behaviors;

    private void ToggleOff()
    {
        _behaviors = GetComponents<Behaviour>();

        foreach (var behavior in _behaviors)
        {
            behavior.enabled = false;
        }
    }
    private void ToggleOn()
    {
        _behaviors = GetComponents<Behaviour>();

        foreach (var behavior in _behaviors)
        {
            behavior.enabled = true;
        }
    }
}