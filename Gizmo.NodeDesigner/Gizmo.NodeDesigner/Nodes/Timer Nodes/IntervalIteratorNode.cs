using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;

namespace Gizmo.NodeDesigner
{
    public class IntervalIteratorNode : Node
    {
        private readonly uint DefaultInterval = 1000; //ms

        private bool IsEnabled = true;
        private bool State;
        private uint Interval;
        private DateTime UpdateTime;
        private decimal IterationValue =0.1m;
        public IntervalIteratorNode()
        {
            NodeStyle = NodeStyleEnum.Default;
            UseIcon = true;
            UseInputs = true;
            UseOutputs = true;
            UseSettings = true;
            Icon = "m16 0c-3.6-1.3e-5 -7.2 1.4-9.9 4.1-5.5 5.5-5.5 14 0 20l2.1-2.1c-2.1-2.1-3.2-4.8-3.2-7.8 7.1e-4 -2.9 1.1-5.7 3.2-7.8 2.1-2.1 4.8-3.2 7.8-3.2 2.9 7.1e-4 5.7 1.1 7.8 3.2 2.1 2.1 3.2 4.8 3.2 7.8 0 2.3-0.68 4.4-1.9 6.2l-1.4-1.3-3.4 7.8 8.2-3-1.2-1.3c4.1-5.5 3.7-13-1.3-18-2.7-2.7-6.3-4.1-9.9-4.1zm4.2 6.9-4.2 4.2-2.8-2.8-2.8 2.8 5.7 5.7 7.1-7.1zm-3.7 16c-0.016 0.23-0.074 0.43-0.18 0.59-0.1 0.16-0.25 0.28-0.46 0.38-0.17 0.082-0.39 0.14-0.64 0.18-0.26 0.031-0.53 0.047-0.81 0.047v1.4h1.8v4.6h-1.8v1.5h5.8v-1.5h-1.8v-7.2zm-9.7 0.99v3.1h-3.1v1.6h3.1v3.1h1.6v-3.1h3.1v-1.6h-3.1v-3.1z";
            NodeName = "ITERATOR";
            NodeCategory = "TIME & TIMERS Nodes";
            NodeDescription = "Генератор сигнала по таймеру.\nИзменение значения с 0 на 1 происходит \nпо истечению половины заданного интервала, \nесли \"Use Zero\" включено; \nЕсли \"Use Zero\" то на выход подается\n всегда 1 с указанным интервалом.";
            Size = new NodeBase.EntitySize(100,90);
            Interval = DefaultInterval;
            UpdateTime = DateTime.Now;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, Index = 0, Name = "dT", Value = Interval, DefaultValue = DefaultInterval, VariableType= VariableType.Setting, DataType = typeof(uint) });
            AddVariable(new Variable() { ParentId = Id, Index = 1, Name = "Inc", Value = IterationValue, DefaultValue = IterationValue, VariableType = VariableType.Setting, DataType = typeof(decimal) });
            AddVariable(new Variable() { ParentId = Id, Index = 0, Name = "Start/Stop", Value = false, DefaultValue = false, VariableType = VariableType.Input, DataType = typeof(bool) });
            AddVariable(new Variable() { ParentId = Id, Index = 0, Name = "Out", Value = (decimal)0, DefaultValue = (decimal)0, VariableType = VariableType.Output, DataType = typeof(decimal) });


        }
        public override void Loop()
        {
            if (!IsEnabled)
                return;

            if ((DateTime.Now - UpdateTime).TotalMilliseconds < Interval)
                return;

            UpdateTime = DateTime.Now;

            Outputs[0].Value = Convert.ToDecimal(Outputs[0].Value) + IterationValue;

        }
        public override void OnInputChanges(Variable input)
        {
            if (input.Id == Inputs[0].Id)
            {
                IsEnabled = (bool)input.Value;
            }
        }
        public override void OnSettingChanges(Variable internalSetting)
        {
            if (internalSetting.Id == Settings[0].Id)
            {
                if (internalSetting.Value == null)
                    Interval = DefaultInterval;
                else
                    Interval = (uint)internalSetting.Value;

                if (Interval < 1)
                    Interval = 1;
            }
            else if (internalSetting.Id == Settings[1].Id)
            {
                IterationValue = (decimal)Settings[1].Value;
            }
        }
    }
}
