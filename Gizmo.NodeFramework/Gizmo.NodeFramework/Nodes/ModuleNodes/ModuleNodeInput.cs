namespace Gizmo.NodeFramework
{
    public class ModuleNodeInput:Node
    {
        public ModuleNodeInput() : base()
        {
            NodeStyle = NodeStyleEnum.Minimalistic;
            NodeCategory = "Module nodes";
            NodeName = "Module Node Input";
            Icon = "m12 9.7v1h7.3v-1zm13-8.8-6.5 9.3 6.5 9.3 6.5-9.3-0.2-0.29zm-19 0-6.5 9.3 6.5 9.3 6.5-9.3-0.2-0.29zm0 1.8 5.3 7.6-5.3 7.6-5.3-7.6zm15 28h-1.3l-3.7-6.9v6.9h-0.96v-7.8h1.6l3.4 6.3v-6.3h0.96zm-7.7 0h-3.1v-0.79h1v-6.2h-1v-0.79h3.1v0.79h-1v6.2h1z";
        }

        public override void Initialize()
        {
            base.Initialize();
            AddVariable(new Variable() { ParentId = Id, VariableType = VariableType.Output, ShowName = true, IsEditable = false, Index = 0 });
        }

        public override bool OnAdd(Engine engine)
        {
            var moduleType = Module.GetModuleType(ModuleId);
            if (moduleType == ModuleType.Init || moduleType == ModuleType.Main || moduleType == ModuleType.Exit)
            {
                engine.AddLogError("Can`t create input for readonly modules.");
                return false;
            }

            var module = engine.GetModuleNode(ModuleId);
            if (module == null)
            {
                engine.AddLogError($"Can`t create module inout. Module does not exist.");
                return false;
            }

            module.AddInputNode(this);

            base.OnAdd(engine);
            return true;
        }


        public override void OnRemove()
        {
            var module = NodeEngine.GetModuleNode(ModuleId);
            module?.RemoveModuleInput(this);
        }
    }
}
