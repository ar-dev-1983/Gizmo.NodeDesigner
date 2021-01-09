using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;

namespace Gizmo.NodeDesigner
{
    public class MathMulNode : Node
    {
        public MathMulNode() : base()
        {
            NodeStyle = NodeStyleEnum.Minimalistic;
            NodeName = "Mul";
            NodeCategory = "Math nodes";
            NodeDescription = "Умножение всех входов.\nВсе входы и выход по-умолчанию Decimal, для изменения типов входов и выходов используйте настройки.";
            UseIcon = true;
            Icon = "m21 8-5 5-5-5-3 3 5 5-5 5 3 3 5-5 5 5 3-3-5-5 5-5z";
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
                Outputs[0].Value = (Decimal)Inputs[0].Value * (Decimal)Inputs[1].Value;
            }
            catch (Exception)
            {
            }
        }
    }
}
