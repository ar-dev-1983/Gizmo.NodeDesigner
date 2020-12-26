using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;
using System.Windows;
using System.Windows.Markup;

namespace Gizmo.Designer.Nodes
{
    public class TickerNode : Node
    {
        private DateTime UpdateTime;
        public TickerNode()
        {
            NodeStyle = NodeStyleEnum.Default;
            UseIcon = true;
            UseInputs = true;
            UseOutputs = true;
            UseSettings = true;
            Icon = "m1.5 26v1.1h1.8v4.7h1.5v-4.7h1.8v-1.1zm5.8 0v1h0.95v3.8h-0.95v1h3.4v-1h-0.95v-3.8h0.95v-1zm4.6 0v5.8h1.4v-3.9l1.1 2.5h1l1.1-2.5v3.9h1.5v-5.8h-1.7l-1.3 2.9-1.3-2.9zm7.6 0v5.8h4.2v-1.1h-2.7v-1.4h2.5v-1.1h-2.5v-1h2.7v-1.1zm5.5 0v5.8h1.5v-2.1h0.69l1.6 2.1h1.8l-1.9-2.5c0.37-0.18 0.65-0.4 0.85-0.68 0.2-0.27 0.3-0.62 0.3-1.1 0-0.31-0.065-0.57-0.2-0.78-0.13-0.21-0.3-0.38-0.52-0.51-0.22-0.13-0.45-0.21-0.7-0.25-0.25-0.039-0.54-0.059-0.89-0.059zm1.5 1.1h0.54c0.18 0 0.34 0.0052 0.47 0.016 0.14 0.0078 0.25 0.033 0.36 0.074 0.15 0.06 0.25 0.14 0.31 0.25 0.06 0.1 0.09 0.23 0.09 0.38 0 0.16-0.025 0.3-0.074 0.39-0.047 0.096-0.12 0.18-0.23 0.27-0.11 0.083-0.24 0.14-0.41 0.16-0.16 0.023-0.36 0.035-0.6 0.035h-0.46zm-7-9.7-4.9-4.9v-6.6h3v5.4l4.1 4.1zm-3.4-18c-6.6 0-12 5.4-12 12s5.4 12 12 12 12-5.4 12-12-5.4-12-12-12zm0 21c-5 0-9-4-9-9s4-9 9-9 9 4 9 9-4 9-9 9z";
            NodeName = "TICKER";
            NodeCategory = "TIME & TIMERS Nodes";
            Size = new NodeBase.EntitySize(100, 90);
            UpdateTime = DateTime.Now;
        }
        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, Index = 0, Name = "dT", Value = (uint)1000, DefaultValue = (uint)1000, VariableType = VariableType.Setting, DataType = typeof(uint) });
            AddVariable(new Variable() { ParentId = Id, Index = 1, Name = "0/1", Value = true, DefaultValue = true, VariableType = VariableType.Setting, DataType = typeof(bool) });
            AddVariable(new Variable() { ParentId = Id, Index = 0, Name = "Enable", Value = true, DefaultValue = true, VariableType = VariableType.Input, DataType = typeof(bool) });
            AddVariable(new Variable() { ParentId = Id, Index = 0, Name = "Out", Value = 1.0m, DefaultValue = 1.0m, VariableType = VariableType.Output });
        }

        public override void EngineStarted()
        {
            base.EngineStarted();
            UpdateTime = DateTime.Now;
            foreach (var item in Settings)
            {
                item.DefaultValue = item.Value;
            }
        }

        public override void Loop()
        {
            if (Inputs[0].Value == null)
                Inputs[0].Value = true;

            if (!(bool)Inputs[0].Value)
                return;

            if ((DateTime.Now - UpdateTime).TotalMilliseconds < (uint)Settings[0].Value)
                return;
            UpdateTime = DateTime.Now;

            Outputs[0].Value = (bool)Settings[1].Value ? 1.0m : 0.0m;
        }

        public override void OnSettingChanges(Variable internalSetting)
        {
            if (internalSetting.Id == Settings[0].Id)
            {
                if (internalSetting.Value == null)
                    Settings[0].Value = 1000;

                if ((uint)internalSetting.Value < 1)
                    Settings[0].Value = 1;
            }
        }
    }
}
