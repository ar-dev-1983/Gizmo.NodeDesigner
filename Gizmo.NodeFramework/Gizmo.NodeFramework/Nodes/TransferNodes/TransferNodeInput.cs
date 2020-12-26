using System;

namespace Gizmo.NodeFramework
{
    public class TransferNodeInput : Node
    {
        private string sourceNodeName = string.Empty;
        private Guid sourceNodeId = Guid.Empty;
        private Guid sourceVariableId = Guid.Empty;

        public TransferNodeInput() : base()
        {
            NodeName = "TO";
            NodeStyle = NodeStyleEnum.Small;
            NodeCategory = "Node to Node communication";
            UseIcon = false;
            Icon = "m12 9.7v1h7.3v-1zm13-8.8-6.5 9.3 6.5 9.3 6.5-9.3-0.2-0.29zm-19 0-6.5 9.3 6.5 9.3 6.5-9.3-0.2-0.29zm0 1.8 5.3 7.6-5.3 7.6-5.3-7.6zm15 28h-1.3l-3.7-6.9v6.9h-0.96v-7.8h1.6l3.4 6.3v-6.3h0.96zm-7.7 0h-3.1v-0.79h1v-6.2h-1v-0.79h3.1v0.79h-1v6.2h1z";
            UseInputs = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, VariableType = VariableType.Input, ShowName = true, IsEditable = false, Index = 0 });
            Inputs[0].OnConnected += TransferNodeInput_OnConnected;
            Inputs[0].OnDisonnected += TransferNodeInput_OnDisonnected;
        }

        private void TransferNodeInput_OnDisonnected(Variable variable)
        {
            if (!variable.IsConnected)
            {
                if (NodeEngine != null)
                {
                    variable.Name = string.Empty;
                    sourceNodeId = Guid.Empty;
                    sourceVariableId = Guid.Empty;
                    sourceNodeName = string.Empty;
                }
            }
        }

        private void TransferNodeInput_OnConnected(Variable variable)
        {
            if (variable.IsConnected)
            {
                if (NodeEngine != null)
                {
                    var link = NodeEngine.GetLinkForInput(variable);
                    var node = NodeEngine.GetNodeByOutputId(link.OutputId);
                    sourceNodeId = node.Id;
                    sourceNodeName = node.Name;
                    sourceVariableId = link.OutputId;
                    variable.Name = node.Name + "." + node.GetOutput(link.OutputId).Name;
                }
            }
        }

        public override bool OnAdd(Engine engine)
        {
            base.OnAdd(engine);
            return true;
        }


        public override void OnRemove()
        {

        }
    }
}
