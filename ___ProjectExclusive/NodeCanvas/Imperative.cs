using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace ___ProjectExclusive.NodeCanvas
{

    [Name("-!- Imperative -!-",10)]
    [Category("Composites")]
    [Description("Returns [Running] until the left node is [Success].\n" +
                 "On Left [Fail]: switch to the " +
                 "right node and will pin it until it finishes.\n" +
                 "On Right [Success]: returns to the left; else returns [Fail]")]
    [Icon("Shield")]
    [Color("7f003f")]
    public class Imperative : BTNode
    {
        public override int maxOutConnections => 2;

        private bool _priorityFailed = false;
        protected override Status OnExecute(Component agent, IBlackboard blackboard)
        {
            Connection priorityConnection = outConnections[0];
            Connection slaveConnection = outConnections[1];

            if (_priorityFailed)
            {
                if (slaveConnection is null)
                    return Status.Failure;

                Status slaveStatus = slaveConnection.Execute(agent, blackboard);
                if (slaveStatus == Status.Success)
                {
                    _priorityFailed = false;
                    priorityConnection.Reset();
                }
                return slaveStatus == Status.Failure
                    ? Status.Failure
                    : Status.Running;
            }

            Status priorityStatus = priorityConnection.Execute(agent, blackboard);
            if (priorityStatus == Status.Failure)
            {
                _priorityFailed = true;
                outConnections[1].Reset();
            }

            return priorityStatus == Status.Success ? Status.Success : Status.Running;
        }

        protected override void OnNodeGUI()
        {
            GUILayout.Label("<b>Imperative</b>");

        }
    }
}
