using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ToggleOfflineMode : MonoBehaviourPun
{
    [SerializeField] private bool isOffline;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("");
    }
}
