//#define PatrolDebug

using UnityEngine;
using UnityEngine.AI;

namespace GoblinStateMachine
{

    public class Patrol : IState
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Entity _entity;

        private float _patrolTime;
        private float _timeToPatrol;
        private Vector3 _newDestination;
        private bool _patrolling;
        private float _patrolSpeed;
        private float _radius;

        public Patrol(Entity entity)
        {
            _entity = entity;
            _navMeshAgent = entity.NavAgent;
            _animator = entity.Animator;
        }

        public void Tick()
        {
            PatrolArea();
        }

        public void FixedTick()
        {
            
        }

        public void OnEnter()
        {
            _timeToPatrol = Random.Range(0, 10);
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _patrolSpeed;
            _patrolTime = _timeToPatrol;
            _patrolling = false;
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
        }

        private void PatrolArea()
        {
            if (_patrolling == false)
                _patrolTime += Time.deltaTime;

            if (_patrolTime >= _timeToPatrol)
            {
                _timeToPatrol = Random.Range(2, 12);
                TriggerPatrol();
                _patrolTime = 0;
            }

            if (Vector3.Distance(_entity.transform.position, _newDestination) < .2f)
            {
                _animator.SetBool("Running", false);
                _patrolling = false;
            }
        }

        private void TriggerPatrol()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _radius;
            randomDirection += _entity.transform.position;
#if PatrolDebug
        Debug.Log("X: " + randomX);
        Debug.Log("Y: " + randomX);
#endif

            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, _radius, 1);
            Vector3 finalPosition = hit.position;
            _newDestination = finalPosition;

#if PatrolDebug
        Debug.Log("New destination is: " + _newDestination);
#endif
            _navMeshAgent.destination = finalPosition;
            _animator.SetBool("Running", true);
            _patrolling = true;
        }
    }
}