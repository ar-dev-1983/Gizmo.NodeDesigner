using System;
using System.Linq;

namespace Gizmo.NodeFramework
{

    public class ModuleNode : Node
    {
        public ModuleNode() : base()
        {
            VisbileInToolbox = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            NodeStyle = NodeStyleEnum.Default;
            NodeCategory = "Module nodes";
            NodeName = "Module Node";
        }

        public override void OnInputChanges(Variable input)
        {
            if (NodeEngine != null)
                NodeEngine.GetNodeByInputId(input.Id).Outputs[0].Value = input.Value;
        }

        public void AddInputNode(ModuleNodeInput node)
        {
            if (Inputs.Any(x => x.Id == node.Id))
                return;

            var input = new Variable
            {
                Id = node.Id,
                VariableType = VariableType.Input,
                Name = NamesHelper.GenerateName(Inputs.Select(x => x.Name).ToList(), "In")
            };
            AddVariable(input);
        }

        public void AddOutputNode(ModuleNodeOutput node)
        {
            if (Outputs.Any(x => x.Id == node.Id))
                return;

            var output = new Variable
            {
                Id = node.Id,
                VariableType = VariableType.Output,
                Name = NamesHelper.GenerateName(Outputs.Select(x => x.Name).ToList(), "Out")
            };
            AddVariable(output);
        }


        public bool RemoveModuleInput(ModuleNodeInput node)
        {
            var input = NodeEngine.GetInput(node.Id);

            RemoveVariable(input);
            return true;
        }

        public bool RemoveModuleOutput(ModuleNodeOutput node)
        {
            var output = NodeEngine.GetOutput(node.Id);
            RemoveVariable(output);
            return true;
        }


        public override bool OnAdd(Engine engine)
        {
            NodeEngine = engine;
            Name = NamesHelper.GenerateName(engine.Modules.Select(x => x.Name).ToList(), "MD");
            base.OnAdd(engine);
            return true;
        }
    }
}
