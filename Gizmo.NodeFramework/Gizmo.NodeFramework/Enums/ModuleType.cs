using System.ComponentModel;

namespace Gizmo.NodeFramework
{
    public enum ModuleType
    {
        [Description("Undefined module")]
        Undefined = 0,
        [Description("Init sequence module")]
        Init = 1,
        [Description("Main sequence module")]
        Main = 2,
        [Description("Exit sequence module")]
        Exit = 3,
        [Description("Module")]
        Unique = 4,
        [Description("Library module")]
        Independent = 99
    }
}
