using Gizmo.WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{
    public class NodeToolbox : ItemsControl, ICorneredControl
    {
        public NodeToolbox()
        : base()
        {
            DefaultStyleKey = typeof(NodeToolbox);
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
        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }
        public Brush CategoryBrush
        {
            get => (Brush)GetValue(CategoryBrushProperty);
            set => SetValue(CategoryBrushProperty, value);
        }
        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(NodeToolbox), new FrameworkPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register("Category", typeof(string), typeof(NodeToolbox), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty CategoryBrushProperty = DependencyProperty.Register("CategoryBrush", typeof(Brush), typeof(NodeToolbox), new FrameworkPropertyMetadata(Brushes.White));
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(NodeToolbox), new FrameworkPropertyMetadata(false));
    }
}
