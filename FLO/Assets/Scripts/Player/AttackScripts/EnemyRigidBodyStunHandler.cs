using UnityEngine;
using UnityEngine.AI;

public class EnemyRigidBodyStunHandler : RigidBodyStunHandler
{
    private void Start()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _enemyAI = GetComponent<EnemyAI>();
        _groundCheck = GetComponent<GroundCheck>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    public override void DisableComponents()
    {
        if (_navMesh)
            _navMesh.enabled = false;
    }
}