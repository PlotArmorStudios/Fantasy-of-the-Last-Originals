using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoblinBehaviorTree;
using UnityEngine.AI;

namespace GoblinBehaviorTree
{
    public class Patrol : Node
    {
        private Transform _transform;
        private Animator _animator;

        private int _currentWaypointIndex = 0;

        private float _waitTime = 1f; // in seconds
        private float _waitCounter = 0f;
        private bool _waiting = false;
        private float _radius;
        private Vector3 _newDestination;
        private NavMeshAgent _navMeshAgent;
        private GoblinBehavior _goblinBehavior;
        private bool _patrolling;
        private float _patrolTime;
        private float _timeToPatrol;

        public Patrol(GoblinBehavior tree)
        {
            _transform = tree.transform;
            _goblinBehavior = tree;
            _animator = tree.GetComponent<Animator>();
            _radius = tree.GetComponent<GoblinBehavior>().HomeRadius - 1;
            _navMeshAgent = tree.NavMeshAgent;
            Debug.Log("Nav agent: " + _navMeshAgent);
        }

        //Perform the patrolling behavior
        public override NodeState Evaluate()
        {
            PatrolArea();
//this state will be running continuously.
            state = NodeState.RUNNING;
            return state;
        }
        
        private void PatrolArea()
        {
            if (_patrolling == false)
                _patrolTime += Time.deltaTime;

            if (_patrolTime >= _timeToPatrol)
            {
                _navMeshAgent.enabled = true;
                _timeToPatrol = Random.Range(2, 12);
                _patrolTime = _timeToPatrol;
                TriggerPatrol();
                _patrolTime = 0;
            }

            if (Vector3.Distance(_transform.transform.position, _newDestination) < .3f)
            {
                _animator.SetBool("Running", false);
                _navMeshAgent.enabled = false;
                _patrolling = false;
            }
        }
        private void TriggerPatrol()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _radius;
            randomDirection += _goblinBehavior.InitialPosition;
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
            _transform.transform.rotation = Quaternion.Slerp(_transform.transform.rotation,
                Quaternion.LookRotation(finalPosition - _transform.transform.position), 50f * Time.deltaTime);
            _navMeshAgent.destination = finalPosition;
            _animator.SetBool("Running", true);
            _patrolling = true;
        }
    }
}