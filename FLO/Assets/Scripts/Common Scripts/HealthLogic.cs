using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthLogic : MonoBehaviour
{
    [SerializeField] protected float _maxHealth = 150f;
    [SerializeField] protected float _currentHealth;

    public float MAXHealthValue => _maxHealth;
    public float CurrentHealthValue => _currentHealth;

    public event Action OnHealthUpdate;

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    [ContextMenu("Take Lethal Damage")]
    public virtual void TakeNonLethalDamage()
    {
        _currentHealth -= _currentHealth;
        OnHealthUpdate?.Invoke();
    }
    
    [ContextMenu("Take Non-Lethal Damage")]
    public virtual void TakeDamage()
    {
        _currentHealth -= 100;
        OnHealthUpdate?.Invoke();
    }
    
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        OnHealthUpdate?.Invoke();
    }
}