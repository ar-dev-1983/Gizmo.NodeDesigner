using Gizmo.NodeFramework;
using Gizmo.WPF;

namespace Gizmo.NodeDesigner
{
    public class UIntegerNode : Node
    {
        public UIntegerNode() : base()
        {
            NodeStyle = NodeStyleEnum.Small;
            NodeName = "UINT";
            NodeCategory = "Data output nodes";
            NodeDescription = "Вывод Unsigned Integer значения.";
            UseIcon = false;
            Icon = "m29 0.96h-2.8v6.8h-1v-6.8h-2.8v-0.92h6.6zm-7.6 6.8h-1.3l-3.7-6.9v6.9h-0.96v-7.8h1.6l3.4 6.3v-6.3h0.96zm-7.7 0h-3.1v-0.79h1v-6.2h-1v-0.79h3.1v0.79h-1v6.2h1zm-4.7-3.1q0 0.84-0.19 1.5-0.18 0.62-0.6 1-0.4 0.4-0.94 0.58-0.54 0.18-1.2 0.18-0.73 0-1.3-0.19-0.54-0.19-0.91-0.57-0.42-0.43-0.61-1-0.18-0.6-0.18-1.5v-4.6h1v4.7q0 0.63 0.083 0.99 0.089 0.36 0.29 0.66 0.23 0.34 0.62 0.51 0.4 0.17 0.95 0.17 0.56 0 0.95-0.17 0.39-0.17 0.62-0.52 0.2-0.3 0.29-0.68 0.089-0.39 0.089-0.95v-4.7h1zm7 4.1-9.1 7.5h5.3v4.5l-11 4.6 15 6.2 15-6.2-11-4.6v-4.5h5.3zm0 1.3 6.3 5.2h-3.6v7.1h-5.4v-7.1h-3.6z";
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
                Value = (uint)0,
                DefaultValue = (uint)0,
                VariableType = VariableType.Output,
                IsEditable = true,
                DataType=typeof(uint)
            };
        }
    }
}
