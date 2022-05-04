using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoblinBehaviorTree;
using UnityEngine.AI;

namespace GoblinBehaviorTree
{
    public class DetectTarget : Node
    {
        private static int _enemyLayerMask = 1 << 6;

        private Transform _transform;
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private GoblinBehavior _goblinBehavior;

        public DetectTarget(Transform transform)
        {
            _transform = transform;
            _goblinBehavior = transform.GetComponent<GoblinBehavior>();
            _animator = transform.GetComponent<Animator>();
            _navMeshAgent = transform.GetComponent<NavMeshAgent>();
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target"); //check if we have a target
            if (t == null)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    _transform.position, _goblinBehavior.DetectionRadius, _enemyLayerMask);

                if (colliders.Length > 0)
                {
                    //If a target is found from the search, store that target in shared data in the root
                    parent.parent.SetData("target", colliders[0].transform);
                    _navMeshAgent.enabled = true;

                    _animator.CrossFade("Running", .25f);
                    _animator.SetBool("Running", true);
                    _animator.SetBool("Attacking", false);
                    state = NodeState.SUCCESS;
                    return state;
                }

//if no target is found, return FAILURE and stop evaluation of this node

                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.SUCCESS; //if target is found, return SUCCESS state
            return state;
        }
    }
}