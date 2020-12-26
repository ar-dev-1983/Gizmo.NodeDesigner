using System.Windows;

namespace Gizmo.NodeFrameworkUI
{
    public class UpDownEventArgs : RoutedEventArgs
    {
        public UpDownDirection Direction { get; private set; }
        public bool UseMouseWheel { get; private set; }

        public UpDownEventArgs(UpDownDirection direction) : base()
        {
            Direction = direction;
        }

        public UpDownEventArgs(RoutedEvent routedEvent, UpDownDirection direction) : base(routedEvent)
        {
            Direction = direction;
        }

        public UpDownEventArgs(UpDownDirection direction, bool useMouseWheel) : base()
        {
            Direction = direction;
            UseMouseWheel = useMouseWheel;
        }

        public UpDownEventArgs(RoutedEvent routedEvent, UpDownDirection direction, bool useMouseWheel) : base(routedEvent)
        {
            Direction = direction;
            UseMouseWheel = useMouseWheel;
        }
    }

}
