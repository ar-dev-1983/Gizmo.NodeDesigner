using System.Windows;

namespace Gizmo.NodeFrameworkUI
{
    internal struct VariableWrapperInfo
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public Size Size { get; set; }
        public Point Position { get; set; }
        public VariableWrapperOrientation Orientation { get; set; }
    }
}
