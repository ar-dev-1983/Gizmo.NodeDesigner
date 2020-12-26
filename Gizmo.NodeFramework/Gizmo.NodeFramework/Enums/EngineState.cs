using System.ComponentModel;

namespace Gizmo.NodeFramework
{
    public enum EngineState
    {
        [Description("Stop Engine")]
        Stop = 0,
        [Description("Start Engine")]
        Start = 1,
        [Description("Play Engine")]
        Play = 2,
        [Description("Pause Engine")]
        Pause = 3,
        [Description("Step Engine")]
        Step = 4
    }
}
