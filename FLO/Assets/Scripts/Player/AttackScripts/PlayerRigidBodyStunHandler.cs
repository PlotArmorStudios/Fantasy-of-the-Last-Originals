using UnityEngine;

public class PlayerRigidBodyStunHandler : RigidBodyStunHandler
{
    private void Start()
    {
        _playerLogic = GetComponent<Player>();
        _groundCheck = GetComponent<GroundCheck>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
}