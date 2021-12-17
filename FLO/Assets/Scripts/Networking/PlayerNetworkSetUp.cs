using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSetUp : MonoBehaviour
{
   [SerializeField] private GameObject _cam;

   public void IsLocalPlayer()
   {
      _cam.SetActive(true);
   }
}
