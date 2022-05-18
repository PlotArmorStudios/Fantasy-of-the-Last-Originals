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
        }

        public void OnEnter()
        {
            _stateMachine.Launch = false;
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
            if (_stunHandler.CurrentDownForce > 3) _stunHandler.CurrentDownForce = 3;
            if (_stunHandler.CurrentDownForce < 0) _stunHandler.CurrentDownForce = 0f;

            if (!_stunHandler.GroundCheck.UpdateIsGrounded())
            {
                ApplyHookSkillGravity();
            }
            else
            {
                NullifyGravity();
            }

            _stunHandler.Rigidbody.velocity = new Vector3(_stunHandler.Rigidbody.velocity.x,
                _stunHandler.Rigidbody.velocity.y - _stunHandler.CurrentDownForce, _stunHandler.Rigidbody.velocity.z);
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
                _stunHandler.FallAccelerationMultiplier += Time.deltaTime * 4f;

                _stunHandler.CurrentDownForce = _stunHandler.FallAccelerationMultiplier *
                                                ((_stunHandler.FallAccelerationMultiplier *
                                                  _stunHandler.FallAccelerationNormalizer) * _stunHandler.Weight);
            }
            else if (_stunHandler.IsFalling)
            {
                _stunHandler.FallDecelerationMultiplier += Time.deltaTime * _stunHandler.Weight;

                _stunHandler.CurrentDownForce = .1f;
            }
        }

        #endregion
    }
}