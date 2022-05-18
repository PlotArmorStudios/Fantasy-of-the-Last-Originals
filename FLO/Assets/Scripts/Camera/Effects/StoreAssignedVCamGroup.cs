using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

internal class StoreAssignedVCamGroup : MonoBehaviour
{
    [SerializeField] private CinemachineStateDrivenCamera _vCam;

    public CinemachineStateDrivenCamera AssignedVCam => _vCam;
    private List<CameraShake> _shakeableCams;

    private void Start()
    {
        _shakeableCams = GetComponentsInChildren<CameraShake>().ToList();
    }

    public void ShakeCams(float intensity, float time)
    {
        foreach (var cam in _shakeableCams)
        {
            cam.ShakeCamera(intensity, time);
        }
    }
}