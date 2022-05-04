using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinBehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected virtual void Start()
        {
            _root = SetupTree(); //the root itself recursively contains the entire tree
        }

        private void Update()
        {
            if (_root != null)
                _root.Evaluate(); //if there is a tree, continuously "tick" or "evaluate"
            Debug.Log(_root);
        }

        protected abstract Node SetupTree();
    }
}