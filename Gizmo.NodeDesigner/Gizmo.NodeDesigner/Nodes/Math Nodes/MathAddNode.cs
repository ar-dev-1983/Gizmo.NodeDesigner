using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;

namespace Gizmo.NodeDesigner
{
    public class MathAddNode : Node
    {
        public MathAddNode() : base()
        {
            NodeStyle = NodeStyleEnum.Minimalistic;
            NodeName = "Add";
            NodeCategory = "Math nodes";
            NodeDescription = "Сумма всех входов.\nВсе входы и выход по-умолчанию Decimal, для изменения типов входов и выходов используйте настройки.";
            UseIcon = true;
            Icon = "m6.8076 18.121v-4.2426h7.0711v-7.0711h4.2426v7.0711h7.0711v4.2426h-7.0711v7.0711h-4.2426v-7.0711z";
            UseOutputs = true;
            UseInputs = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, Index = 0, IsConnected = false, Name = "Out", Value = 0.0m, DefaultValue = 0.0m, VariableType = VariableType.Output, DataType = typeof(decimal) });
            AddVariable(new Variable() { ParentId = Id, Index = 0, IsConnected = false, Name = "In0", Value = 0.0m, DefaultValue = 0.0m, VariableType = VariableType.Input,  DataType = typeof(decimal) });
            AddVariable(new Variable() { ParentId = Id, Index = 1, IsConnected = false, Name = "In1", Value = 0.0m, DefaultValue = 0.0m, VariableType = VariableType.Input,  DataType = typeof(decimal) });
        }

        public override void Loop()
        {
            base.Loop();
            try
            {
                Outputs[0].Value = (Decimal)Inputs[0].Value + (Decimal)Inputs[1].Value;
            }
            catch (Exception)
            {
            }
        }
    }
}
