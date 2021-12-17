using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MagicalFX;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
   private CinemachineFreeLook _cinemachineVCam;
   private float _shakeTimer;
   private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

   private void Awake()
   {
      _cinemachineVCam = GetComponent<CinemachineFreeLook>();
   }

   public void ShakeCamera(float intensity, float time)
   {
      _multiChannelPerlin =
         _cinemachineVCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

     _multiChannelPerlin.m_AmplitudeGain = intensity;
     _shakeTimer = time;
   }

   private void Update()
   {
      if (_shakeTimer > 0)
      {
         _shakeTimer -= Time.deltaTime;

         if (_shakeTimer <= 0f)
         {
            CinemachineBasicMultiChannelPerlin _multiChannelPerlin =
               _cinemachineVCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            _multiChannelPerlin.m_AmplitudeGain = 0f;
         }
      }
   }
}
