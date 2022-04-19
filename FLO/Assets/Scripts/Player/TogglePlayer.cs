using System;
using System.Collections;
using UnityEngine;

public class TogglePlayer : ToggleBehaviors
{
    private CombatManager _combatManager;
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
        _combatManager = GetComponent<CombatManager>();
    }

    public void TogglePlayerOff()
    {
        _player.enabled = false;
        _combatManager.enabled = false;
    }

    public void TogglePlayerOn()
    {
        _player.enabled = true;
        _combatManager.enabled = true;
    }
}

public class ToggleBehaviors : MonoBehaviour
{
    private Behaviour[] _behaviors;

    public void ToggleOff()
    {
        _behaviors = GetComponents<Behaviour>();

        foreach (var behavior in _behaviors)
        {
            if (behavior is Animator) return;
            behavior.enabled = false;
        }
    }

    public void ToggleOn()
    {
        _behaviors = GetComponents<Behaviour>();

        foreach (var behavior in _behaviors)
        {
            behavior.enabled = true;
        }
    }
}