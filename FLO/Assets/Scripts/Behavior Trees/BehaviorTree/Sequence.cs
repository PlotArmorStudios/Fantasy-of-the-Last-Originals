using System.Collections.Generic;

namespace GoblinBehaviorTree
{
    //Composite node that acts like an "AND" logic gate
    //Only if all child nodes success will it succeed itself
    public class Sequence : Node 
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            //iterate through the children and check their state after evaluation
            //If any child fails we can stop there and return the fail state
            foreach (Node node in children) 
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
//Check if any children are still running or if all have succeeded
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }

    }

}
