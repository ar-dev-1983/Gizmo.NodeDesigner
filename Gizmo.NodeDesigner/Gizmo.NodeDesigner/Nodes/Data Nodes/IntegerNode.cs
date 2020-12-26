using Gizmo.NodeFramework;
using Gizmo.WPF;

namespace Gizmo.NodeDesigner
{
    public class IntegerNode : Node
    {
        public IntegerNode() : base()
        {
            NodeStyle = NodeStyleEnum.Small;
            NodeName = "INT";
            NodeCategory = "Data output nodes";
            NodeDescription = "Вывод Integer значения.";
            UseIcon = false;
            Icon = "m25 0.96h-2.8v6.8h-1v-6.8h-2.8v-0.92h6.6zm-7.6 6.8h-1.3l-3.7-6.9v6.9h-0.96v-7.8h1.6l3.4 6.3v-6.3h0.96zm-7.7 0h-3.1v-0.79h1v-6.2h-1v-0.79h3.1v0.79h-1v6.2h1zm6.1 0.94-9.1 7.5h5.3v4.5l-11 4.6 15 6.2 15-6.2-11-4.6v-4.5h5.3zm0 1.3 6.3 5.2h-3.6v7.1h-5.4v-7.1h-3.6z";
            UseOutputs = true;
            AddOutputsAllowed = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(GetReferenceVariable());
        }

        public override void Loop()
        {
            base.Loop();
            foreach (var variable in Outputs)
            {
                variable.Value = variable.Value;
            }
        }

        public override Variable GetReferenceVariable()
        {
            return new Variable()
            {
                ParentId = Id,
                Index = Outputs.Count,
                Name = "Out" + Outputs.Count.ToString(),
                Value = (int)0,
                DefaultValue = (int)0,
                VariableType = VariableType.Output,
                IsEditable = true,
                DataType=typeof(int)
            };
        }
    }
}
