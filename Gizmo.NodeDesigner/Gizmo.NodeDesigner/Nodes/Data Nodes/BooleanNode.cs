using Gizmo.NodeFramework;

namespace Gizmo.NodeDesigner
{
    public class BooleanNode : Node
    {
        public BooleanNode() : base()
        {
            NodeStyle = NodeStyleEnum.Small;
            NodeName = "BOOL";
            NodeCategory = "Data output nodes";
            NodeDescription = "Вывод Boolean значения.";
            UseIcon = false;
            Icon = "m31 8h-4.9v-7.8h1v6.8h3.9zm-7.5-6.9q0.47 0.52 0.72 1.3 0.26 0.76 0.26 1.7 0 0.96-0.26 1.7-0.26 0.76-0.72 1.3-0.48 0.53-1.1 0.79-0.65 0.27-1.5 0.27-0.82 0-1.5-0.27-0.67-0.27-1.1-0.79-0.47-0.52-0.72-1.3-0.25-0.75-0.25-1.7 0-0.95 0.25-1.7 0.25-0.76 0.73-1.3 0.46-0.51 1.1-0.78 0.68-0.27 1.5-0.27 0.83 0 1.5 0.28 0.67 0.27 1.1 0.78zm-0.094 3q0-1.5-0.68-2.3-0.68-0.82-1.8-0.82-1.2 0-1.9 0.82-0.67 0.82-0.67 2.3 0 1.5 0.69 2.3 0.69 0.81 1.8 0.81t1.8-0.81q0.69-0.81 0.69-2.3zm-8.3-3q0.47 0.52 0.72 1.3 0.26 0.76 0.26 1.7 0 0.96-0.26 1.7-0.26 0.76-0.72 1.3-0.48 0.53-1.1 0.79-0.65 0.27-1.5 0.27-0.82 0-1.5-0.27-0.67-0.27-1.1-0.79-0.47-0.52-0.72-1.3-0.25-0.75-0.25-1.7 0-0.95 0.25-1.7 0.25-0.76 0.73-1.3 0.46-0.51 1.1-0.78 0.68-0.27 1.5-0.27 0.83 0 1.5 0.28 0.67 0.27 1.1 0.78zm-0.094 3q0-1.5-0.68-2.3-0.68-0.82-1.8-0.82-1.2 0-1.9 0.82-0.67 0.82-0.67 2.3 0 1.5 0.69 2.3 0.69 0.81 1.8 0.81t1.8-0.81q0.69-0.81 0.69-2.3zm-7.2 1.5q0 0.58-0.22 1-0.22 0.44-0.59 0.73-0.44 0.34-0.96 0.49-0.52 0.15-1.3 0.15h-2.8v-7.8h2.3q0.85 0 1.3 0.062t0.81 0.26q0.43 0.22 0.62 0.58 0.19 0.35 0.19 0.84 0 0.55-0.28 0.94-0.28 0.39-0.75 0.62v0.042q0.79 0.16 1.2 0.69 0.45 0.53 0.45 1.3zm-1.7-3.5q0-0.28-0.094-0.47t-0.3-0.31q-0.24-0.14-0.59-0.17-0.35-0.036-0.86-0.036h-1.2v2.2h1.3q0.48 0 0.77-0.047 0.29-0.052 0.53-0.21 0.24-0.16 0.34-0.4 0.1-0.25 0.1-0.59zm0.66 3.5q0-0.47-0.14-0.74t-0.51-0.47q-0.25-0.13-0.61-0.17-0.35-0.042-0.86-0.042h-1.6v2.9h1.4q0.68 0 1.1-0.068 0.43-0.073 0.71-0.26 0.29-0.2 0.43-0.46 0.14-0.26 0.14-0.67zm9.7 3.1-9.1 7.5h5.3v4.5l-11 4.6 15 6.2 15-6.2-11-4.6v-4.5h5.3zm0 1.3 6.3 5.2h-3.6v7.1h-5.4v-7.1h-3.6z";
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
                Value = false,
                DefaultValue = false,
                VariableType = VariableType.Output,
                IsEditable = true,
                DataType = typeof(bool)
            };
        }
    }
}
