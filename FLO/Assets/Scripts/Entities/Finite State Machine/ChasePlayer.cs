using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : IState
{
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Player _player;
    private float _attackTimer;
    private Animator _animator;
    private Entity _entity;

    public ChasePlayer(Entity entity, Player player, NavMeshAgent navMeshAgent)
    {
        _entity = entity;
        _player = player;

        _navMeshAgent = _entity.NavAgent;
        _animator = _entity.Animator;
    }

    public void Tick()
    {
        FollowPlayer();
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        
        _animator.CrossFade("Running", .25f);
        _animator.SetBool("Running", true);
        _animator.SetBool("Attacking", false);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
    }

    void FollowPlayer()
    {
        _navMeshAgent.isStopped = false;
        _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation,
            Quaternion.LookRotation(_player.transform.position - _entity.transform.position), 5f * Time.deltaTime);
        
        _navMeshAgent.SetDestination(_player.transform.position);
    }
}