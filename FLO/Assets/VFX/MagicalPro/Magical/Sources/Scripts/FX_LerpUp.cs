using System;
using UnityEngine;

public class FX_LerpUp : MonoBehaviour
{
    [SerializeField] private float _lerpHeight = 2f;
    [SerializeField] private float _animationTime = .5f;
    private Vector3 _newPosition;
    private bool _atPosition;

    private void Start()
    {
        _newPosition = new Vector3(transform.position.x, transform.position.y + _lerpHeight, transform.position.z);
        _atPosition = false;
    }

    private void Update()
    {
        if (!_atPosition)
        {
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime / _animationTime);
            _atPosition = true;
        }
    }
}