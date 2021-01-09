using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;

namespace Gizmo.NodeDesigner
{
    public class MathDivNode : Node
    {
        public MathDivNode() : base()
        {
            NodeStyle = NodeStyleEnum.Minimalistic;
            NodeName = "Div";
            NodeCategory = "Math nodes";
            NodeDescription = "Деление всех входов.\nВсе входы и выход по-умолчанию Decimal, для изменения типов входов и выходов используйте настройки.";
            UseIcon = true;
            Icon = "m16 6.8a2.6 2.6 0 0 0-2.6 2.6 2.6 2.6 0 0 0 2.6 2.6 2.6 2.6 0 0 0 2.6-2.6 2.6 2.6 0 0 0-2.6-2.6zm-9.2 7.1v4.2h18v-4.2h-18zm9.2 6a2.6 2.6 0 0 0-2.6 2.6 2.6 2.6 0 0 0 2.6 2.6 2.6 2.6 0 0 0 2.6-2.6 2.6 2.6 0 0 0-2.6-2.6z";
            UseOutputs = true;
            UseInputs = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, Index = 0, IsConnected = false, Name = "Out", Value = 0.0m, DefaultValue = 0.0m, VariableType = VariableType.Output, DataType = typeof(decimal) });
            AddVariable(new Variable() { ParentId = Id, Index = 0, IsConnected = false, Name = "In0", Value = 0.0m, DefaultValue = 0.0m, VariableType = VariableType.Input, DataType = typeof(decimal) });
            AddVariable(new Variable() { ParentId = Id, Index = 1, IsConnected = false, Name = "In1", Value = 0.0m, DefaultValue = 0.0m, VariableType = VariableType.Input, DataType = typeof(decimal) });
        }

        public override void Loop()
        {
            base.Loop();
            try
            {
                Outputs[0].Value = (Decimal)Inputs[0].Value / (Decimal)Inputs[1].Value;
            }
            catch (Exception)
            {
            }
        }
    }
}
