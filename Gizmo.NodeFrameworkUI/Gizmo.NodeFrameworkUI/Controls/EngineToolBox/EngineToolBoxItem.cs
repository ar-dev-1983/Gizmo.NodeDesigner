using Gizmo.WPF;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.NodeFrameworkUI
{
    public class EngineToolBoxItem : Button, ICorneredControl
    {
        public EngineToolBoxItem()
 : base()
        {
            DefaultStyleKey = typeof(EngineToolBoxItem);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(EngineToolBoxItem), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(bool), typeof(EngineToolBoxItem), new FrameworkPropertyMetadata(false));
    }
}
