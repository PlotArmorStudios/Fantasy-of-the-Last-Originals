using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePlayerControl : MonoBehaviour
{
    [SerializeField] private bool _activateOnEnter = true;
    [SerializeField] private bool _deactivateOnExit = true;

    private void OnEnable()
    {
        if (_activateOnEnter) PauseMenu.Active = true;
    }

    private void OnDisable()
    {
        if(_deactivateOnExit) PauseMenu.Active = false;
    }
}