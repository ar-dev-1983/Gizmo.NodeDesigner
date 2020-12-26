using System.Windows;
using System.Windows.Controls;

namespace Gizmo.NodeDesigner
{
    public class ProjectItemIcon : ContentControl
    {
        public ProjectItemIcon()
: base()
        {
            DefaultStyleKey = typeof(ProjectItemIcon);
        }
        static ProjectItemIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectItemIcon), new FrameworkPropertyMetadata(typeof(ProjectItemIcon)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        public ProjectItemType ItemType
        {
            get => (ProjectItemType)GetValue(ItemTypeProperty);
            set => SetValue(ItemTypeProperty, value);
        }
        public bool ShowSelectionDot
        {
            get => (bool)GetValue(ShowSelectionDotProperty);
            set => SetValue(ShowSelectionDotProperty, value);
        }
        public static readonly DependencyProperty ItemTypeProperty = DependencyProperty.Register("ItemType", typeof(ProjectItemType), typeof(ProjectItemIcon), new UIPropertyMetadata(ProjectItemType.None));
        public static readonly DependencyProperty ShowSelectionDotProperty = DependencyProperty.Register("ShowSelectionDot", typeof(bool), typeof(ProjectItemIcon), new UIPropertyMetadata(false));
    }
}
