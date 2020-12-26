using Gizmo.NodeFramework;
using Gizmo.WPF;

namespace Gizmo.NodeDesigner
{
    public class DecimalNode : Node
    {
        public DecimalNode() : base()
        {
            NodeStyle = NodeStyleEnum.Small;
            NodeName = "DEC";
            NodeCategory = "Data output nodes";
            NodeDescription = "Вывод Decimal значения.";
            UseIcon = false;
            Icon = "m24-2.5e-7c-0.59 0-1.1 0.09-1.6 0.27-0.48 0.18-0.89 0.44-1.2 0.78-0.35 0.35-0.62 0.79-0.81 1.3-0.19 0.51-0.28 1.1-0.28 1.7 0 0.69 0.097 1.3 0.29 1.8 0.19 0.52 0.46 0.94 0.81 1.3 0.35 0.34 0.77 0.6 1.2 0.76 0.48 0.16 1 0.24 1.6 0.24 0.31 0 0.59-0.026 0.85-0.076 0.26-0.046 0.5-0.1 0.72-0.17 0.26-0.081 0.47-0.16 0.62-0.23 0.16-0.074 0.34-0.15 0.54-0.24v-1.2h-0.077c-0.39 0.35-0.81 0.61-1.3 0.78-0.45 0.17-0.93 0.26-1.4 0.26-0.37 0-0.73-0.063-1.1-0.19-0.33-0.13-0.63-0.32-0.88-0.58-0.24-0.25-0.44-0.58-0.59-0.98-0.15-0.4-0.23-0.88-0.23-1.4 0-0.53 0.067-1 0.2-1.4 0.14-0.41 0.33-0.74 0.58-1 0.25-0.27 0.54-0.47 0.87-0.6 0.33-0.13 0.69-0.19 1.1-0.19 0.5 0 0.96 0.082 1.4 0.25 0.43 0.17 0.87 0.43 1.3 0.79h0.081v-1.2c-0.52-0.26-1-0.44-1.4-0.54-0.43-0.1-0.85-0.15-1.3-0.15zm-19 0.14v7.9h2c0.74 0 1.3-0.05 1.8-0.15 0.45-0.099 0.86-0.26 1.2-0.48 0.53-0.32 0.95-0.77 1.3-1.4 0.32-0.58 0.48-1.2 0.48-2 0-0.77-0.15-1.4-0.46-2-0.3-0.56-0.73-1-1.3-1.3-0.32-0.19-0.71-0.35-1.2-0.46-0.46-0.11-1.1-0.17-1.9-0.17zm8.5 0v7.9h5.3v-0.93h-4.2v-2.9h4.2v-0.93h-4.2v-2.2h4.2v-0.93zm-7.4 0.9h1c0.5 0 0.92 0.037 1.3 0.11 0.36 0.067 0.69 0.19 1 0.37 0.43 0.24 0.76 0.58 0.98 1 0.22 0.42 0.33 0.94 0.33 1.6 0 0.61-0.1 1.1-0.3 1.5-0.2 0.41-0.49 0.74-0.89 0.99-0.32 0.2-0.67 0.34-1.1 0.41-0.38 0.074-0.83 0.11-1.3 0.11h-1zm9.8 7.8-9.4 7.6h5.5v4.6l-11 4.7 15 6.3 15-6.3-11-4.7v-4.6h5.5zm0 1.3 6.5 5.3h-3.7v7.2h-5.6v-7.2h-3.7z";
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
                Value = 0.0m,
                DefaultValue = 0.0m,
                VariableType = VariableType.Output,
                IsEditable = true,
                DataType=typeof(decimal)
            };
        }
    }
}
