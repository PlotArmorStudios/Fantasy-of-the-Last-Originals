using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera MultiPlayerVCam;
    [field: SerializeField] public bool MultiPlayer;
    
    private List<Controller> _controllers;
    public static int AssignedControllers;
    public static bool MultiplePlayers => AssignedControllers >= 2;
    private void Awake()
    {
        _controllers = FindObjectsOfType<Controller>().ToList();
        int index = 1;
        
        foreach (var controller in _controllers)
        {
            controller.SetIndex(index);
            index++;
        }
    }

    private void Start()
    {
        foreach (var controller in _controllers)
        {
            if (controller.IsAssigned == false)
            {
                AssignController(controller);
            }
        }
    }

    private void Update()
    {
        foreach (var controller in _controllers)
        {
            if (controller.IsAssigned == false && controller.AnyButtonDown())
            {
                AssignController(controller);
            }
        }

        MultiPlayer = MultiplePlayers;
    }

    private void AssignController(Controller controller)
    {
        controller.IsAssigned = true;
        Debug.Log("Assigned Controller " + controller.gameObject.name);
        FindObjectOfType<PlayerManager>().AddPlayerToGame(controller);
        AssignedControllers++;

        if (MultiplePlayers && MultiPlayerVCam)
            MultiPlayerVCam.Priority = 50;
        else if(MultiPlayerVCam)
            MultiPlayerVCam.Priority = -1;
    }
}