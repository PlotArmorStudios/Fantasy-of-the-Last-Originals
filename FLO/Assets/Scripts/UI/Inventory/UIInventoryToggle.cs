using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class UIInventoryToggle : MonoBehaviour
{
    public event Action OnTurnOnInventory;
    public event Action OnTurnOffInventory;
    
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
        ToggleInventoryOff();
    }


    void Update()
    {
        ToggleInventory();
    }

    private void ToggleInventoryOn()
    {
        _panelState = PanelStates.On;
        _storedFreeLook.m_XAxis.m_InputAxisValue = 0f;
        _storedFreeLook.m_YAxis.m_InputAxisValue = 0f;
        _storedFreeLook.m_XAxis.m_InputAxisName = null;
        _storedFreeLook.m_YAxis.m_InputAxisName = null;
        _inventoryPanel.SetActive(true);
    }

    private void ToggleInventoryOff()
    {
        _panelState = PanelStates.Off;
        _storedFreeLook.m_XAxis.m_InputAxisName = "Mouse X";
        _storedFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
        _inventoryPanel.SetActive(false);
    }

    void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_panelState == PanelStates.Off) ToggleInventoryOn();
            else ToggleInventoryOff();
        }
    }
}