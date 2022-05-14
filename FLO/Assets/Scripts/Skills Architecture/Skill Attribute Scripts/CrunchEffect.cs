using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class CrunchEffect : MonoBehaviour, IEffect
{
    [SerializeField] private GameObject _rock;
    [SerializeField] private GameObject _crunchEffect;
    [SerializeField] private float _intensity = 4f;
    [SerializeField] private float _time = .3f;

    private AudioSource _source;
    private CameraShake _camShake;
    public PlayerAttackDefinitionManager AssignedPlayer { get; set; }

    public void PlayEffect()
    {
        _source = _rock.GetComponent<AudioSource>();
        _source.Play();

        AssignedPlayer = GetComponent<EffectAttackDefinitionManager>().AssignedPlayer;
        _camShake = AssignedPlayer.GetComponent<StoreAssignedVCamGroup>().AssignedVCam
            .GetComponentInChildren<CameraShake>();
        _camShake.ShakeCamera(_intensity, _time);

        _rock.GetComponent<MeshRenderer>().enabled = false;
        InstantiateMethod();
    }

    private void InstantiateMethod()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Instantiate(_crunchEffect.name, transform.position, Quaternion.identity);
        else
            Instantiate(_crunchEffect, transform.position, Quaternion.identity, transform);
    }
}