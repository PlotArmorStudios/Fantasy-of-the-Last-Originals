using UnityEngine;
using UnityEngine.AI;

public class EnemyRigidBodyStunHandler : RigidBodyStunHandler
{
    private void Start()
    {
        Entity = GetComponent<Entity>();
        GroundCheck = GetComponent<GroundCheck>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public override void DisableComponents()
    {
    }
}
