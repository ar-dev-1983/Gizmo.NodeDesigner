namespace Gizmo.NodeFramework
{
    public class ModuleNodeOutput : Node
    {
        public ModuleNodeOutput() : base()
        {
            NodeStyle = NodeStyleEnum.Small;
            NodeCategory = "Module nodes";
            NodeName = "Module Node Output";
            Icon = "m6.5 0.75-6.5 9.3 6.5 9.3 6.1-8.8h6.7l6.1 8.8 6.5-9.3-6.5-9.3-6.1 8.8h-6.7zm19 1.8 5.3 7.6-5.3 7.6-5.3-7.6zm-17 20c-0.53 0-1 0.091-1.5 0.27-0.45 0.18-0.83 0.44-1.1 0.78-0.32 0.36-0.56 0.79-0.73 1.3-0.17 0.5-0.25 1.1-0.25 1.7 0 0.64 0.083 1.2 0.25 1.7 0.17 0.5 0.41 0.92 0.72 1.3s0.69 0.61 1.1 0.79c0.45 0.18 0.95 0.27 1.5 0.27 0.56 0 1.1-0.089 1.5-0.27 0.44-0.18 0.82-0.44 1.1-0.79 0.31-0.34 0.55-0.76 0.72-1.3 0.17-0.51 0.26-1.1 0.26-1.7 0-0.64-0.084-1.2-0.25-1.7-0.17-0.5-0.41-0.93-0.72-1.3-0.31-0.34-0.69-0.59-1.1-0.78-0.44-0.18-0.94-0.28-1.5-0.28zm5.1 0.16v4.6c0 0.59 0.06 1.1 0.18 1.5 0.12 0.4 0.33 0.75 0.61 1 0.25 0.25 0.55 0.44 0.91 0.57 0.36 0.13 0.79 0.19 1.3 0.19 0.48 0 0.89-0.062 1.2-0.18 0.36-0.12 0.67-0.31 0.94-0.58 0.28-0.28 0.48-0.62 0.6-1 0.12-0.42 0.19-0.91 0.19-1.5v-4.6h-1v4.7c-1e-6 0.38-0.029 0.7-0.088 0.95-0.056 0.25-0.15 0.48-0.29 0.68-0.16 0.23-0.36 0.4-0.62 0.52-0.26 0.11-0.58 0.17-0.95 0.17-0.37 0-0.69-0.057-0.95-0.17-0.26-0.11-0.47-0.28-0.62-0.51-0.14-0.2-0.23-0.42-0.29-0.66-0.056-0.24-0.084-0.57-0.084-0.99v-4.7zm6.9 0v0.92h2.8v6.8h1v-6.8h2.8v-0.92zm-12 0.73c0.78 0 1.4 0.27 1.8 0.82 0.45 0.55 0.68 1.3 0.68 2.3 0 1-0.23 1.8-0.69 2.3-0.45 0.54-1.1 0.81-1.8 0.81-0.77 0-1.4-0.27-1.8-0.81-0.46-0.54-0.69-1.3-0.69-2.3 0-1 0.22-1.8 0.67-2.3 0.45-0.55 1.1-0.82 1.9-0.82z";
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, VariableType = VariableType.Input, ShowName = true, IsEditable = false, Index = 0 });
        }

        public override void OnInputChanges(Variable input)
        {
            if (NodeEngine != null)
                NodeEngine.GetOutput(Id).Value = input.Value;
        }

        public override bool OnAdd(Engine engine)
        {
            var moduleType = Module.GetModuleType(ModuleId);
            if (moduleType == ModuleType.Init || moduleType == ModuleType.Main || moduleType == ModuleType.Exit)
            {
                engine.AddLogError("Can`t create output for readonly modules.");
                return false;
            }

            var module = engine.GetModuleNode(ModuleId);
            if (module == null)
            {
                engine.AddLogError($"Can`t create module output. Module does not exist.");
                return false;
            }

            module.AddOutputNode(this);

            base.OnAdd(engine);
            return true;
        }


        public override void OnRemove()
        {
            var module = NodeEngine.GetModuleNode(ModuleId);
            module?.RemoveModuleOutput(this);
        }
    }
}
