using Gizmo.NodeFramework;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.NodeDesigner
{
    public class LogMessageIcon : ContentControl
    {
        public LogMessageIcon()
: base()
        {
            DefaultStyleKey = typeof(LogMessageIcon);
        }
        static LogMessageIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LogMessageIcon), new FrameworkPropertyMetadata(typeof(LogMessageIcon)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        public LogMessageType ItemType
        {
            get => (LogMessageType)GetValue(ItemTypeProperty);
            set => SetValue(ItemTypeProperty, value);
        }

        public static readonly DependencyProperty ItemTypeProperty = DependencyProperty.Register("ItemType", typeof(LogMessageType), typeof(LogMessageIcon), new UIPropertyMetadata(LogMessageType.Information));
    }
}
