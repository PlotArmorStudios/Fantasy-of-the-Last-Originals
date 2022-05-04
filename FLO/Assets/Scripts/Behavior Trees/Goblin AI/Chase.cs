using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoblinBehaviorTree;
using UnityEngine.AI;

namespace GoblinBehaviorTree
{
    public class Chase : Node
    {
        private Transform _transform;
        private NavMeshAgent _navMeshAgent;
        private Transform _target;

        public Chase(Transform transform)
        {
            _transform = transform;
            _navMeshAgent = _transform.GetComponent<NavMeshAgent>();
        }

        public override NodeState Evaluate()
        {
            _target = (Transform) GetData("target"); // get the target from target data slot in root of tree

            FollowPlayer();

            state = NodeState.RUNNING;
            return state;
        }
        
        void FollowPlayer()
        {
            _navMeshAgent.isStopped = false;
            _transform.transform.rotation = Quaternion.Slerp(_transform.transform.rotation,
                Quaternion.LookRotation(_target.transform.position - _transform.transform.position), 5f * Time.deltaTime);

            _navMeshAgent.SetDestination(_target.transform.position);
        }
    }
}