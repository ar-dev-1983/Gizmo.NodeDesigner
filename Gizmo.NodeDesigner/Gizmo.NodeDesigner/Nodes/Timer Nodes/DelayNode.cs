using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace Gizmo.Designer.Nodes
{
    public class DelayNode : Node
    {
        private List<DataStruct> Values = new List<DataStruct>();
        private readonly UInt32 DefaultInterval = 1000; //ms
        private UInt32 Interval;

        public DelayNode()
        {
            NodeStyle = NodeStyleEnum.Default;
            UseIcon = true;
            UseInputs = true;
            UseOutputs = true;
            UseSettings = true;
            Icon = "m30 26-2.1 3.6v2.2h-1.5v-2.2l-2.2-3.6h1.7l1.3 2.2 1.2-2.2zm-6 5.8h-1.6l-0.4-1.2h-2.2l-0.4 1.2h-1.5l2.1-5.8h1.7zm-2.3-2.2-0.71-2.1-0.71 2.1zm-4 2.2h-4.2v-5.8h1.5v4.7h2.7zm-5.5 0h-4.2v-5.8h4.2v1.1h-2.7v1h2.5v1.1h-2.5v1.4h2.7zm-5.4-2.9q0 0.81-0.37 1.5-0.37 0.64-0.94 0.98-0.43 0.26-0.93 0.36-0.51 0.1-1.2 0.1h-2.1v-5.8h2.1q0.71 0 1.2 0.12 0.52 0.12 0.87 0.34 0.6 0.37 0.95 0.99 0.35 0.62 0.35 1.5zm-1.6-0.012q0-0.57-0.21-0.98-0.21-0.41-0.66-0.64-0.23-0.11-0.47-0.15-0.24-0.043-0.72-0.043h-0.38v3.6h0.38q0.54 0 0.79-0.047 0.25-0.051 0.49-0.18 0.41-0.23 0.6-0.62 0.19-0.39 0.19-0.97zm14-12-4.9-4.9v-6.6h3v5.4l4.1 4.1zm-3.4-18c-6.6 0-12 5.4-12 12s5.4 12 12 12 12-5.4 12-12-5.4-12-12-12zm0 21c-5 0-9-4-9-9s4-9 9-9 9 4 9 9-4 9-9 9z";
            NodeName = "Delay";
            NodeCategory = "TIME & TIMERS Nodes";
            Size = new NodeBase.EntitySize(100, 70);
            Interval = DefaultInterval;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, Index = 0, Name = "dT", Value = Interval, DefaultValue = DefaultInterval, VariableType = VariableType.Setting, DataType = typeof(uint) });
            AddVariable(new Variable() { ParentId = Id, Index = 0, IsConnected = false, Name = "In", Value = null, DefaultValue = null, VariableType = VariableType.Input });
            AddVariable(new Variable() { ParentId = Id, Index = 0, IsConnected = false, Name = "Out", Value = null, DefaultValue = null, VariableType = VariableType.Output });
        }

        public override void Loop()
        {
            foreach (var node in Values)
            {
                if ((DateTime.Now - node.TimeStamp).TotalMilliseconds >= Interval)
                {
                    Values.Remove(node);
                    Outputs[0].Value = node.Value;
                    return;
                }
            }
        }

        public override void OnInputChanges(Variable input)
        {
            if (input == Inputs[0])
            {
                Values.Add(new DataStruct(input.Value, DateTime.Now, 100));
            }
        }

        public override void OnSettingChanges(Variable internalSetting)
        {
            if (internalSetting.Id == Settings[0].Id)
            {
                if (internalSetting.Value == null)
                    Interval = DefaultInterval;
                else
                    Interval = (UInt32)internalSetting.Value;

                if (Interval < 1)
                    Interval = 1;
            }
        }
    }
}
