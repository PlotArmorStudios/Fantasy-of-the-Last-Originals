using UnityEngine;

public class PlayerRigidBodyStunHandler : RigidBodyStunHandler
{
    private void Start()
    {
        GroundCheck = GetComponent<GroundCheck>();
        Rigidbody = GetComponent<Rigidbody>();
    }
}