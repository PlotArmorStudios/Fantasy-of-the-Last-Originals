using System.Collections;
using System.Collections.Generic;

namespace GoblinBehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

//Represents a single element in a tree and can access both its children and parents
//Has a node state
//Can store, retrieve, or clear shared data
    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        //Shared data
        //A map of data that can be stored.
        //Can be stored as any type
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children) //creates the edge between a node and its new child(ren)
        {
            foreach (Node child in children)
                _Attach(child);
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE; //inheriting from this will allow the nodes to have their unique roles in the behavior tree

        public void SetData(string key, object value) //Add to data set
        {
            _dataContext[key] = value;
        }

        //Getting the data is a bit more complex
        //We want to check if the data is defined somewhere in our branch
        //Not just in this particular node
        //This will make it easier to access and use shared data in our behavior tree
        
        //Recursively work up the branch until we've found the key we were looking for
        //Or until we've reached the root of the tree
        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }

            return null;
        }

        //Recursively work up the branch until we've found the key we want to delete
        //Or until we've reached the root of the tree
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }

            return false;
        }
    }
}