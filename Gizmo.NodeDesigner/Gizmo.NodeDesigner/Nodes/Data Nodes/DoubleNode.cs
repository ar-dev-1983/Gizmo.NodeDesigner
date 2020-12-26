using Gizmo.NodeFramework;
using Gizmo.WPF;

namespace Gizmo.NodeDesigner
{
    public class DoubleNode : Node
    {
        public DoubleNode() : base()
        {
            NodeStyle = NodeStyleEnum.Small;
            NodeName = "DBL";
            NodeCategory = "Data output nodes";
            NodeDescription = "Вывод Double значения.";
            UseIcon = false;
            Icon = "m26 7.8h-4.9v-7.8h1v6.8h3.9zm-6.4-2.4q0 0.58-0.22 1-0.22 0.44-0.59 0.73-0.44 0.34-0.96 0.49-0.52 0.15-1.3 0.15h-2.8v-7.8h2.3q0.85 0 1.3 0.063 0.42 0.062 0.81 0.26 0.43 0.22 0.62 0.58 0.19 0.35 0.19 0.84 0 0.55-0.28 0.94-0.28 0.39-0.75 0.62v0.042q0.79 0.16 1.2 0.69 0.45 0.53 0.45 1.3zm-1.7-3.5q0-0.28-0.094-0.47-0.094-0.19-0.3-0.31-0.24-0.14-0.59-0.17-0.35-0.036-0.86-0.036h-1.2v2.2h1.3q0.48 0 0.77-0.047 0.29-0.052 0.53-0.21 0.24-0.16 0.34-0.4 0.1-0.25 0.1-0.59zm0.66 3.5q0-0.47-0.14-0.74-0.14-0.28-0.51-0.47-0.25-0.13-0.61-0.17-0.35-0.042-0.86-0.042h-1.6v2.9h1.4q0.68 0 1.1-0.068 0.43-0.073 0.71-0.26 0.29-0.2 0.43-0.46 0.14-0.26 0.14-0.67zm-6.5-1.5q0 1.1-0.46 1.9-0.46 0.86-1.2 1.3-0.53 0.33-1.2 0.47-0.65 0.15-1.7 0.15h-2v-7.8h1.9q1.1 0 1.8 0.17 0.67 0.16 1.1 0.45 0.79 0.49 1.2 1.3 0.44 0.82 0.44 2zm-1.1-0.016q0-0.91-0.32-1.5-0.32-0.62-0.95-0.98-0.46-0.26-0.97-0.36-0.52-0.1-1.2-0.1h-0.97v6h0.97q0.74 0 1.3-0.11 0.56-0.11 1-0.41 0.58-0.37 0.86-0.97 0.29-0.6 0.29-1.5zm4.6 5-9.4 7.6h5.5v4.6l-11 4.7 15 6.3 15-6.3-11-4.7v-4.6h5.5zm0 1.3 6.5 5.3h-3.7v7.2h-5.6v-7.2h-3.7z";
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
                Value = 0.0d,
                DefaultValue = 0.0d,
                VariableType = VariableType.Output,
                IsEditable = true,
                DataType=typeof(double)
            };
        }
    }
}
