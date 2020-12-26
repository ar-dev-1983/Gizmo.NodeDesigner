﻿using Gizmo.NodeFramework;

namespace Gizmo.NodeDesigner
{
    public class ValueNode : Node
    {
        public ValueNode() : base()
        {
            NodeStyle = NodeStyleEnum.Small;
            NodeName = "VAL";
            NodeCategory = "Data visualisation nodes";
            NodeDescription = "Вывод значения.";
            UseIcon = false;
            Icon = "m29 0.45c-1.8 0-3.2 0.56-4.3 1.7-1.1 1.1-1.7 2.6-1.7 4.4 1e-6 1.7 0.49 3 1.5 4 0.98 1 2.3 1.5 4.1 1.5 1.3 1e-6 2.4-0.2 3.2-0.59v-2.3c-0.82 0.48-1.7 0.72-2.7 0.72-1 0-1.9-0.32-2.5-0.96-0.61-0.65-0.91-1.5-0.91-2.6-3.3e-5 -1.1 0.32-2 0.97-2.7 0.65-0.66 1.5-0.99 2.5-0.99 0.96 0 1.8 0.22 2.6 0.67v-2.4c-0.75-0.28-1.7-0.41-2.8-0.41zm-24 0.19-4.1 11h2.7l0.79-2.5h4l0.8 2.5h2.8l-4-11h-3zm8.4 0v11h4.3c1.3 0 2.3-0.3 3-0.9 0.73-0.6 1.1-1.4 1.1-2.4 0-0.7-0.24-1.3-0.72-1.8-0.47-0.49-1.1-0.78-1.9-0.88v-0.031c0.64-0.18 1.2-0.49 1.6-0.95 0.41-0.45 0.62-0.98 0.62-1.6-1e-6 -0.83-0.34-1.5-1-1.9-0.67-0.46-1.6-0.69-2.9-0.69h-4.1zm2.5 1.9h0.95c1.2 0 1.8 0.41 1.8 1.2 0 0.44-0.15 0.79-0.45 1-0.3 0.25-0.71 0.38-1.2 0.38h-1.1v-2.7zm-9.5 0.14h0.062c0.036 0.39 0.099 0.73 0.19 1l1.2 3.8h-2.9l1.2-3.8c0.1-0.32 0.17-0.65 0.2-0.98zm9.5 4.4h1.4c0.58 0 1 0.13 1.4 0.38 0.33 0.26 0.49 0.61 0.49 1.1 0 0.47-0.17 0.84-0.5 1.1-0.33 0.27-0.78 0.41-1.4 0.41h-1.4v-3zm-9.7 13-4.8 0.98v2l2.4-0.52v7h-2.3v2h7.1v-2h-2.3v-9.4zm7.8 0c-1.2 0-2.3 0.32-3.3 0.95v2.2c0.88-0.77 1.8-1.1 2.8-1.1 1.1 0 1.7 0.52 1.7 1.5 0 0.46-0.13 0.91-0.4 1.3-0.26 0.42-0.71 0.93-1.4 1.6l-3.2 3.1v1.9h7.4v-2h-4.5v-0.047l2.3-2c1.6-1.4 2.4-2.7 2.4-4.1 0-0.98-0.32-1.8-0.97-2.3-0.64-0.58-1.5-0.87-2.7-0.87zm13 0c-1 0-2 0.2-2.8 0.59v2c0.71-0.48 1.4-0.73 2.2-0.73 1.1 0 1.7 0.46 1.7 1.4 0 0.97-0.73 1.5-2.2 1.5h-0.93v1.9h1c0.76 1e-6 1.4 0.14 1.8 0.41 0.44 0.27 0.66 0.66 0.66 1.2 0 0.48-0.18 0.86-0.53 1.1-0.35 0.27-0.84 0.41-1.5 0.41-1 0-1.9-0.27-2.6-0.82v2.1c0.72 0.38 1.7 0.56 2.8 0.56 1.3 0 2.4-0.3 3.2-0.91 0.77-0.61 1.1-1.4 1.1-2.5 0-0.69-0.23-1.3-0.69-1.8-0.46-0.48-1.1-0.77-1.9-0.87v-0.039c1.5-0.37 2.2-1.3 2.2-2.8 0-0.82-0.32-1.5-0.96-2-0.64-0.52-1.5-0.77-2.7-0.77zm-6.5 9c-0.42 0-0.77 0.12-1 0.36-0.28 0.24-0.41 0.55-0.41 0.92 2e-5 0.36 0.14 0.67 0.41 0.92 0.27 0.25 0.61 0.38 1 0.38 0.44 0 0.79-0.12 1.1-0.36 0.28-0.24 0.41-0.56 0.41-0.94 0-0.37-0.14-0.67-0.41-0.91-0.27-0.24-0.61-0.37-1-0.37z";
            UseInputs = true;
            AddInputsAllowed = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(GetReferenceVariable());
        }

        public override Variable GetReferenceVariable()
        {
            return new Variable() { ParentId = Id, Index = Inputs.Count, IsConnected = false, Name = "Val" + Inputs.Count.ToString(), VariableType = VariableType.Input, ShowValue = true };
        }
    }
}
