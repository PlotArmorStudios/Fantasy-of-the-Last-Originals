using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPhysics : MonoBehaviour
{
    [SerializeField] float _stoppingDistance = 4f;
    [SerializeField] float _attackSlideDistance = 5f;
    [SerializeField] private StopMovementOnCollision _stopMovement;

    private GameObject _player;

    private CharacterController _controller;
    private Rigidbody _rigidbody;
    private AutoTargetEnemy _enemyTargeter;

    public bool StopMovement;
    public GameObject Enemy => _enemyTargeter.TargetedEnemy;
    public bool EnemyIsTooClose => Vector3.Distance(transform.position, Enemy.transform.position) <
                                   _stoppingDistance;

    bool _sliding = false;

    private WaitForSeconds _slideDuration;
    private float _newDuration;
    
    //Attack Upforce
    private bool _applyUpForce;
    private float _upForce;

    private void Start()
    {
        _slideDuration = new WaitForSeconds(_newDuration);
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        _enemyTargeter = GetComponent<AutoTargetEnemy>();
    }

    private void Update()
    {
        if (_stopMovement.isActiveAndEnabled)
            StopMovement = _stopMovement.ShouldStop;
        else
            StopMovement = false;
        
        if (_sliding && !StopMovement) //applies forward movement to character controller when m_sliding is true
        {
            AttackSlide(AttackSlideDistance(_attackSlideDistance));
        }
    }

    private void FixedUpdate()
    {
        ApplyUpForce();
    }
    
    public void ApplyUpForce()
    {
        if (_applyUpForce)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y + _upForce, _rigidbody.velocity.z);
        }
    }

    public void ApplyUpwardForce(float force)
    {
        _upForce = force;
        StartCoroutine(ReverseGravity());
    }

    private IEnumerator ReverseGravity()
    {
        _applyUpForce = true;
        yield return new WaitForSeconds(.2f);
        _applyUpForce = false;
    }
    
    void AttackSlide(float distance)
    {
        transform.position += transform.forward * Time.deltaTime * distance;
    }

    public float AttackSlideDistance(float distance)
    {
        return distance;
    }

    void ReadAttackDistanceValue()
    {
        _attackSlideDistance = AttackSlideDistance(_attackSlideDistance);
    }

    IEnumerator SlideDuration(float slideDuration)
    {
        if (StopMovement)
            yield break;

        _sliding = true;
        _newDuration = slideDuration;
        yield return new WaitForSeconds(slideDuration);
        _sliding = false;
    }
}