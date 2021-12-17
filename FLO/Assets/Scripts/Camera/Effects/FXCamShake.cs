using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXCamShake : MonoBehaviour
{
   private CameraShake _camShake;
   [SerializeField] private float _intensity = 1f;
   [SerializeField] private float _time = 1f;
   private PlayerAttackDefinitionManager AssignedPlayer { get; set; }

   private void Start()
   {
      AssignedPlayer = GetComponent<EffectAttackDefinitionManager>().AssignedPlayer;
      _camShake = AssignedPlayer.GetComponent<StoreAssignedVCamGroup>().AssignedVCam.GetComponentInChildren<CameraShake>();
      _camShake.ShakeCamera(_intensity, _time);
   }

}