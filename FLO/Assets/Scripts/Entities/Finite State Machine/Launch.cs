using System;
using System.Collections;
using UnityEngine;

namespace EntityStates
{
    [System.Serializable]
    public class Launch : IState
    {
        private EntityStateMachine _stateMachine;
        private Entity _entity;
        private Animator _animator;
        private Rigidbody _rigidbody;

        private RigidBodyStunHandler _stunHandler;

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
            if (_stunHandler.GroundCheck.UpdateIsGrounded())
                _stunHandler.DownPull = _stunHandler.StartDownPull;
            if (_entity.IsGrounded) _stateMachine.Launch = false;
        }

        public void FixedTick()
        {
            // _stunHandler.LimitFallAccelerationMultiplier();
            // _stunHandler.ApplyHitStop();
            // CalculateKnockBackPhysics();
        }

        public void OnEnter()
        {
            _stateMachine.Launch = false;
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