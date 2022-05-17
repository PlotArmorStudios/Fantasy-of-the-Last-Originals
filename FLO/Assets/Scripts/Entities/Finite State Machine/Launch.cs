using System;
using UnityEngine;

namespace EntityStates
{
    [System.Serializable]
    public class Launch : IState
    {
        [SerializeField] private float _weight = 3f;
        private EntityStateMachine _stateMachine;
        private Entity _entity;
        private Animator _animator;
        private Rigidbody _rigidbody;
        
        //Physics Calculations
        public bool IsRaising => _rigidbody.velocity.y > 0;
        public bool IsFalling => _rigidbody.velocity.y < 0;
        public float CurrentDownForce { get; set; }

        private float _fallAccelerationMultiplier;
        private float _fallDecelerationMultiplier;
        private float _fallAccelerationNormalizer;
        private float _fallDecelerationNormalizer;
        
        //Link Skill knock back
        private Vector3 ContactPointLaunchLimiter;
        private float _downPull = .05f;
        
        private SkillType _targetSkillTypeUsed;
        private GroundCheck _groundCheck;
        private RigidBodyStunHandler _stunHandler;

        //Base knock back
        public Vector3 KnockBackForce { get; set; }

        public bool IsAboveContactPoint { get; set; }
        public Launch(Entity entity)
        {
            _entity = entity;
            _animator = entity.Animator;
            _stateMachine = entity.GetComponent<EntityStateMachine>();
            _rigidbody = entity.GetComponent<Rigidbody>();
            _stunHandler = entity.GetComponent<RigidBodyStunHandler>();
        }

        public void Tick()
        {
            if (_entity.IsGrounded)
                _stateMachine.Launch = false;
        }

        public void OnEnter()
        {
            _rigidbody.velocity = _stunHandler.KnockBackForce;
            _animator.CrossFade("Flyback Stun", .25f, 0);
        }

        public void OnExit()
        {
            _animator.SetTrigger("Landing");
        }
        
        #region PhysicsCalculations
        
        private void CalculateKnockBackPhysics()
        {
            //Limit Down force
            if (CurrentDownForce > 3) CurrentDownForce = 3;

            if (!_groundCheck.UpdateIsGrounded())
            {
                if (_targetSkillTypeUsed == SkillType.LinkSkill)
                {
                    ApplyLinkSkillGravity();
                }
                else if (_targetSkillTypeUsed != SkillType.LinkSkill)
                {
                    ApplyHookSkillGravity();
                }
            }
            else
            {
                NullifyGravity();
            }

            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y - CurrentDownForce, _rigidbody.velocity.z);
        }

        private void NullifyGravity()
        {
            _fallAccelerationMultiplier = 0;
            _fallDecelerationMultiplier = 0;
            CurrentDownForce = 0;
        }

        private void ApplyHookSkillGravity()
        {
            if (IsRaising)
            {
                _fallAccelerationMultiplier += Time.fixedDeltaTime * 4f;

                CurrentDownForce = _fallAccelerationMultiplier *
                                   ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);
            }

            else if (IsFalling)
            {
                _fallDecelerationMultiplier += Time.fixedDeltaTime * _weight;

                CurrentDownForce += Time.fixedDeltaTime *
                                    ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                                     _weight); //down force is going to decrease over time, and decrease more over time due to fallmult

                if (CurrentDownForce > 3) CurrentDownForce = 3;
                if (CurrentDownForce < 0) CurrentDownForce = 0f;
            }
        }
        
        private void ApplyLinkSkillGravity()
        {
            IsAboveContactPoint = _rigidbody.transform.position.y >= ContactPointLaunchLimiter.y;

            if (_rigidbody.transform.position.y >= ContactPointLaunchLimiter.y)
            {
                _fallAccelerationMultiplier += Time.fixedDeltaTime * 3f;

                CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                                   ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);
            }
            else if (_rigidbody.transform.position.y < ContactPointLaunchLimiter.y)
            {
                if (IsRaising)
                {
                    _fallAccelerationMultiplier += Time.fixedDeltaTime * 3f;

                    CurrentDownForce = _downPull * _fallAccelerationMultiplier *
                                       ((_fallAccelerationMultiplier * _fallAccelerationNormalizer) * _weight);
                }
                else if (IsFalling)
                {
                    _fallDecelerationMultiplier = _fallAccelerationMultiplier > 2f ? 100f : 0f;
                    _fallDecelerationMultiplier += Time.fixedDeltaTime * _weight;
                    CurrentDownForce -=
                        Time.deltaTime *
                        ((_fallDecelerationMultiplier * _fallDecelerationNormalizer) *
                         _weight); //down force is going to decrease over time, and decrease more over time due to fallmult

                    if (CurrentDownForce < 0) CurrentDownForce = 0f;
                }
            }
        }
        #endregion
    }
}
