using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class AttackDefinitionManager : MonoBehaviour
{
    public AttackDefinition CurrentAttackDefinition { get; set; }
    public HitBox ActiveHitBox { get; set; }

    public GameObject ActiveObject;
    private GameObject _effect;
    private PhotonView _view;

    private void Start()
    {
        _view = PhotonView.Get(this);
    }

    public void SetAttackDefinition(AttackDefinition attackDefinition)
    {
        CurrentAttackDefinition = attackDefinition;
    }

    public void SetActiveHitBox()
    {
        SwitchHitBoxActiveState();
    }

    public void SwitchHitBoxActiveState()
    {
        ActiveHitBox.gameObject.SetActive(!ActiveHitBox.gameObject.activeSelf);
    }


    public void PlayVFX()
    {
        PositionVFX();
    }
    
    private void PositionVFX()
    {
        if (ActiveHitBox.EffectPosition == null) return;

        InstantiateMethod();
        _effect.transform.parent = gameObject.transform;


        _effect.transform.position = ActiveHitBox.EffectPosition.position;

        _effect.transform.localPosition += new Vector3(CurrentAttackDefinition.EffectPositionX,
            CurrentAttackDefinition.EffectPositionY, CurrentAttackDefinition.EffectPositionZ);

        _effect.transform.parent = null;

        _effect.transform.localRotation = ActiveHitBox.EffectPosition.rotation;

        _effect.transform.Rotate(Vector3.right, CurrentAttackDefinition.EffectRotationX);
        _effect.transform.Rotate(Vector3.up, CurrentAttackDefinition.EffectRotationY);
        _effect.transform.Rotate(Vector3.forward, CurrentAttackDefinition.EffectRotationZ);

        _effect.transform.localScale += new Vector3(CurrentAttackDefinition.EffectScaleX,
            CurrentAttackDefinition.EffectScaleY, CurrentAttackDefinition.EffectScaleZ);

        if (_effect.GetComponent<ActiveObject>())
            ActiveObject = _effect.gameObject;

        if (_effect.GetComponent<EffectAttackDefinitionManager>())
            _effect.GetComponent<EffectAttackDefinitionManager>().AssignedPlayer = (PlayerAttackDefinitionManager) this;
    }

    private void InstantiateMethod()
    {
        if (PhotonNetwork.IsConnected)
        {
            _effect = PhotonNetwork.Instantiate(CurrentAttackDefinition.Effect.name,
                ActiveHitBox.EffectPosition.position, Quaternion.identity);
        }
        else
            _effect = Instantiate(CurrentAttackDefinition.Effect);
    }
}