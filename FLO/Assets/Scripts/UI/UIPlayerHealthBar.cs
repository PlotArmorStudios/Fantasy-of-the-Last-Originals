using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UIPlayerHealthBar : UIHealthBar
{
    [SerializeField] private Player _player;

    void Start()
    {
        _health = _player.GetComponent<PlayerHealth>();
        _health.OnHealthUpdate += HandleUpdateHealth;
        HandleUpdateHealth();
    }
}