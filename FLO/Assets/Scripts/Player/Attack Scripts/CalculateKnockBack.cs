using UnityEngine;

public class CalculateKnockBack : MonoBehaviour
{
        private RigidBodyStunHandler _stunHandler;
        private bool IsAboveContactPoint;
        private float LaunchPosition { get; set; }

    void Start()
    {
        _stunHandler = GetComponent<RigidBodyStunHandler>();
        _stunHandler.Trajectory = new Vector3(0, 0, 0);
    }

    private void OnValidate()
    {
        if (!GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (_stunHandler.GroundCheck.UpdateIsGrounded())
        {
            _stunHandler.DownPull = _stunHandler.StartDownPull;
            if (_stunHandler.Entity != null)
                _stunHandler.Entity.SetIdle();
        }
    }

    void FixedUpdate()
    {
        _stunHandler.LimitFallAccelerationMultiplier();
        _stunHandler.ApplyHitStop();
        CalculateKnockBackPhysics();
    }

    private void CalculateKnockBackPhysics()
    {
        //Limit Down force
        if (_stunHandler.CurrentDownForce > 3) _stunHandler.CurrentDownForce = 3;

        if (!_stunHandler.GroundCheck.UpdateIsGrounded())
        {
            if (_stunHandler.TargetSkillTypeUsed == SkillType.LinkSkill)
            {
                ApplyLinkSkillGravity();
            }
            else if (_stunHandler.TargetSkillTypeUsed != SkillType.LinkSkill)
            {
                ApplyHookSkillGravity();
            }
        }
        else
        {
            NullifyGravity();
        }

        _stunHandler.Rigidbody.velocity = new Vector3(_stunHandler.Rigidbody.velocity.x, _stunHandler.Rigidbody.velocity.y - _stunHandler.CurrentDownForce, _stunHandler.Rigidbody.velocity.z);
    }

    private void NullifyGravity()
    {
        _stunHandler.FallAccelerationMultiplier = 0;
        _stunHandler.FallDecelerationMultiplier = 0;
        _stunHandler.CurrentDownForce = 0;
    }

    private void ApplyHookSkillGravity()
    {
        if (_stunHandler.IsRaising)
        {
            _stunHandler.FallAccelerationMultiplier += Time.fixedDeltaTime * 4f;

            _stunHandler.CurrentDownForce = _stunHandler.FallAccelerationMultiplier *
                                            ((_stunHandler.FallAccelerationMultiplier * _stunHandler.FallAccelerationNormalizer) * _stunHandler.Weight);
        }

        else if (_stunHandler.IsFalling)
        {
            _stunHandler.FallDecelerationMultiplier += Time.fixedDeltaTime * _stunHandler.Weight;

            _stunHandler.CurrentDownForce += Time.fixedDeltaTime *
                                             ((_stunHandler.FallDecelerationMultiplier * _stunHandler.FallDecelerationNormalizer) *
                                              _stunHandler.Weight); //down force is going to decrease over time, and decrease more over time due to fallmult

            if (_stunHandler.CurrentDownForce > 3) _stunHandler.CurrentDownForce = 3;
            if (_stunHandler.CurrentDownForce < 0) _stunHandler.CurrentDownForce = 0f;
        }
    }
    
    private void ApplyLinkSkillGravity()
    {
        IsAboveContactPoint = _stunHandler.Rigidbody.transform.position.y >= _stunHandler.ContactPointLaunchLimiter.y;

        if (_stunHandler.Rigidbody.transform.position.y >= _stunHandler.ContactPointLaunchLimiter.y)
        {
            _stunHandler.FallAccelerationMultiplier += Time.fixedDeltaTime * 3f;

            _stunHandler.CurrentDownForce = _stunHandler.DownPull * _stunHandler.FallAccelerationMultiplier *
                                            ((_stunHandler.FallAccelerationMultiplier * _stunHandler.FallAccelerationNormalizer) * _stunHandler.Weight);
        }
        else if (_stunHandler.Rigidbody.transform.position.y < _stunHandler.ContactPointLaunchLimiter.y)
        {
            if (_stunHandler.IsRaising)
            {
                _stunHandler.FallAccelerationMultiplier += Time.fixedDeltaTime * 3f;

                _stunHandler.CurrentDownForce = _stunHandler.DownPull * _stunHandler.FallAccelerationMultiplier *
                                                ((_stunHandler.FallAccelerationMultiplier * _stunHandler.FallAccelerationNormalizer) * _stunHandler.Weight);
            }
            else if (_stunHandler.IsFalling)
            {
                _stunHandler.FallDecelerationMultiplier = _stunHandler.FallAccelerationMultiplier > 2f ? 100f : 0f;
                _stunHandler.FallDecelerationMultiplier += Time.fixedDeltaTime * _stunHandler.Weight;
                _stunHandler.CurrentDownForce -=
                    Time.fixedDeltaTime *
                    ((_stunHandler.FallDecelerationMultiplier * _stunHandler.FallDecelerationNormalizer) *
                     _stunHandler.Weight); //down force is going to decrease over time, and decrease more over time due to fallmult

                if (_stunHandler.CurrentDownForce < 0)_stunHandler.CurrentDownForce = 0f;
            }
        }
    }

}