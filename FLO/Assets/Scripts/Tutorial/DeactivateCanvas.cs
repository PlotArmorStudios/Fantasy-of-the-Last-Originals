using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _canvasToDeactivate;

    public void Deactivate()
    {
        _canvasToDeactivate.SetActive(false);
    }
}
