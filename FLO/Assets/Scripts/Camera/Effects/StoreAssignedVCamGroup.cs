using Cinemachine;
using UnityEngine;

internal class StoreAssignedVCamGroup : MonoBehaviour
{
    [SerializeField] private CinemachineStateDrivenCamera _vCam;

    public CinemachineStateDrivenCamera AssignedVCam => _vCam;
}