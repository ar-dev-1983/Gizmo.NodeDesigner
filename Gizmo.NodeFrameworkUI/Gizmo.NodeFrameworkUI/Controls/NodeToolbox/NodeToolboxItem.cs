using Gizmo.NodeFramework;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gizmo.NodeFrameworkUI
{
    public class NodeToolboxItem : ContentControl
    {
        private Point? dragStartPoint = null;

        static NodeToolboxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeToolboxItem), new FrameworkPropertyMetadata(typeof(NodeToolboxItem)));
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public string NodeName
        {
            get => (string)GetValue(NodeNameProperty);
            set => SetValue(NodeNameProperty, value);
        }
        public Type NodeType
        {
            get => (Type)GetValue(NodeTypeProperty);
            set => SetValue(NodeTypeProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(NodeToolboxItem), new FrameworkPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty NodeNameProperty = DependencyProperty.Register("NodeName", typeof(string), typeof(NodeToolboxItem), new FrameworkPropertyMetadata(string.Empty));
        public static readonly DependencyProperty NodeTypeProperty = DependencyProperty.Register("NodeType", typeof(Type), typeof(NodeToolboxItem), new FrameworkPropertyMetadata(null));

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            dragStartPoint = new Point?(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
                dragStartPoint = null;

            if (dragStartPoint.HasValue)
            {
                var NodeInstance = Activator.CreateInstance(NodeType) as Node;
                NodeInstance.Initialize();
                DragObject dataObject = new DragObject
                {
                    TypeName = NodeType.FullName,
                    node = (NodeInstance as Node)
                };
                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
                e.Handled = true;
            }
        }
    }
}
