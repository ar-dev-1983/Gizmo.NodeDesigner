using Gizmo.NodeBase;
using System;

namespace Gizmo.NodeFramework
{
    public class NodeSizeHelper
    {
        public static EntitySize GetDefaultSize(NodeStyleEnum style) => style switch
        {
            NodeStyleEnum.Small => new EntitySize(120,20),
            NodeStyleEnum.Minimalistic => new EntitySize(40, 40),
            NodeStyleEnum.Default => new EntitySize(100, 90),
            NodeStyleEnum.DataVisualizationContainer => new EntitySize(240, 120),
            _ => throw new NotImplementedException()
        };
    }
}
