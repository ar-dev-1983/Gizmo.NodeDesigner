using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;
using System.Windows;
using System.Windows.Markup;

namespace Gizmo.Designer.Nodes
{
    public class DelayMeterNode : Node
    {
        private DateTime UpdateTime;
        public DelayMeterNode()
        {
            NodeStyle = NodeStyleEnum.Minimalistic;
            UseIcon = true;
            UseInputs = true;
            UseOutputs = true;
            UseSettings = true;
            Icon = "m16 0c-8.837 0-16 7.163-16 16s7.163 16 16 16 16-7.163 16-16-7.163-16-16-16zm-6.536 26.067c0.347-0.957 0.536-1.99 0.536-3.067 0-3.886-2.463-7.197-5.913-8.456 0.319-2.654 1.508-5.109 3.427-7.029 2.267-2.266 5.28-3.515 8.485-3.515s6.219 1.248 8.485 3.515c1.92 1.92 3.108 4.375 3.428 7.029-3.45 1.26-5.913 4.57-5.913 8.456 0 1.077 0.189 2.11 0.536 3.067-1.928 1.258-4.18 1.933-6.536 1.933s-4.608-0.675-6.536-1.933zm7.778-6.036c0.434 0.109 0.758 0.503 0.758 0.969v2c0 0.55-0.45 1-1 1h-2c-0.55 0-1-0.45-1-1v-2c0-0.466 0.324-0.86 0.758-0.969l0.742-14.031h1l0.742 14.031z";
            NodeName = "DELAY_METER";
            NodeCategory = "TIME & TIMERS Nodes";
            UpdateTime = DateTime.Now;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, Index = 0, IsConnected = false, Name = "In", Value = null, DefaultValue = null, VariableType = VariableType.Input });
            AddVariable(new Variable() { ParentId = Id, Index = 0, IsConnected = false, Name = "Out", Value = null, DefaultValue = null, VariableType = VariableType.Output });
        }

        public override void OnInputChanges(Variable input)
        {
            if (input.Id == Inputs[0].Id)
            {
                if (input.Value == null)
                    return;

                var Delay = Math.Round((DateTime.Now - UpdateTime).TotalMilliseconds, 0);
                UpdateTime = DateTime.Now;

                Outputs[0].Value = Delay;
            }
        }
    }

}
