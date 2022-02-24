using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackDecelaration : MonoBehaviour
{
    [SerializeField] float _attackForce = 0f;
    [SerializeField] float _startDecelarationMultiplier = 10;

    public bool IsInKnockBack => _isInKnockBack;

    private Rigidbody _rigidBody;

    private bool _isInKnockBack;
    private float _decelerationMultiplier = 0f;
    private float _decelerationDuration = 1f;
    private bool _wasHit;
    private float _hitStopDuration;
    private bool _hasReachedMaxVelocity;
    private float _zVelocity = 10;
    private bool _slowDown;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rigidBody != null)
            if (_isInKnockBack)
            {
                Decelerate();
                StartCoroutine(SetIsKnockBackFalse());
            }
    }

    void Decelerate()
    {
        //knock time set by attack
        _decelerationMultiplier += Time.deltaTime * 2;

        if (_rigidBody.velocity.magnitude > 2)
        {
            if (_rigidBody.velocity.z > 0)
            {
                if (_rigidBody.velocity.magnitude > 4)
                {
                    _slowDown = true;
                }

                if (_slowDown)
                {
                    _zVelocity = _startDecelarationMultiplier * _decelerationMultiplier *
                                 (_decelerationMultiplier * .6f);
                    
                    _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _rigidBody.velocity.y,
                        Mathf.Lerp(_rigidBody.velocity.z, 6, Time.deltaTime * _zVelocity));
                }
            }

            if (_rigidBody.velocity.z < 0)
            {
                if (_rigidBody.velocity.magnitude > 4)
                {
                    _slowDown = true;
                }

                if (_slowDown)
                {
                    _zVelocity = _startDecelarationMultiplier * _decelerationMultiplier *
                                 (_decelerationMultiplier * .6f);
                    _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _rigidBody.velocity.y,
                        Mathf.Lerp(_rigidBody.velocity.z, -6, Time.deltaTime * _zVelocity));
                }
            }
        }

        //_rigidBody.isKinematic = true; //set kinematic back to true
    }

    IEnumerator SetIsKnockBackFalse()
    {
        yield return new WaitForSeconds(_decelerationDuration);
        _isInKnockBack = false;
        _slowDown = false;
        _decelerationMultiplier = 0f;
    }

    IEnumerator SetHitStopDuration()
    {
        yield return new WaitForSeconds(_hitStopDuration);
        _wasHit = false;
    }

    public void SetKnockBackTrue(bool isInKnockBack)
    {
        _isInKnockBack = isInKnockBack;
    }


    public void SetKnockBackDeceleration(float velocity)
    {
        _attackForce = velocity;
        _startDecelarationMultiplier = velocity;
    }

    public void SetHitStop(bool wasHit, float duration)
    {
        _wasHit = wasHit;
        _hitStopDuration = duration;
    }

    public void SetDecelerationDuration(float duration)
    {
        _decelerationDuration = duration;
    }
}