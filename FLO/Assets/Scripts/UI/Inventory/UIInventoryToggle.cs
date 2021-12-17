using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class UIInventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Player _player;

    private StoreAssignedVCamGroup _storedVCam;
    private CinemachineFreeLook _storedFreeLook;

    private enum PanelStates
    {
        On,
        Off
    }

    private string _storeXString;
    private string _storeYString;
    private PanelStates _panelState;

    private void Start()
    {
        _storedVCam = _player.GetComponent<StoreAssignedVCamGroup>();
        _storedFreeLook = _storedVCam.GetComponentInChildren<CinemachineFreeLook>();
        _storeXString = _storedFreeLook.m_XAxis.m_InputAxisName;
        _storeYString = _storedFreeLook.m_YAxis.m_InputAxisName;
        _panelState = PanelStates.Off;
    }


    void Update()
    {
        ToggleInventory();
        _player.enabled = !_inventoryPanel.activeSelf;
    }

    void ToggleInventory()
    {
        switch (_panelState)
        {
            case PanelStates.Off:
                _storedFreeLook.m_XAxis.m_InputAxisName = _storeXString;
                _storedFreeLook.m_YAxis.m_InputAxisName = _storeYString;
                _inventoryPanel.SetActive(false);
                break;
            case PanelStates.On:
                _storedFreeLook.m_XAxis.m_InputAxisValue = 0f;
                _storedFreeLook.m_YAxis.m_InputAxisValue = 0f;
                _storedFreeLook.m_XAxis.m_InputAxisName = null;
                _storedFreeLook.m_YAxis.m_InputAxisName = null;
                _inventoryPanel.SetActive(true);
                break;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_panelState == PanelStates.Off)
                _panelState = PanelStates.On;
            else if (_panelState == PanelStates.On)
            {
                _panelState = PanelStates.Off;
            }

        }
    }
}
