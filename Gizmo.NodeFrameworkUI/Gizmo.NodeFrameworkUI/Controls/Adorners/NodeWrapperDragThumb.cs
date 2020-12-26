using Gizmo.WPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Gizmo.NodeFrameworkUI
{
    public class NodeWrapperDragThumb : Thumb
    {
        public NodeWrapperDragThumb()
  : base()
        {
            DefaultStyleKey = typeof(NodeWrapperDragThumb);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            base.DragDelta += new DragDeltaEventHandler(DragThumb_DragDelta);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(NodeWrapperDragThumb), new FrameworkPropertyMetadata(new CornerRadius(0)));

        void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            NodeWrapper Gizmo = DataContext as NodeWrapper;
            NodeCanvas nodeDesigner = VisualHelper.FindParent<NodeCanvas>(Gizmo);
            if (Gizmo != null && nodeDesigner != null && Gizmo.IsSelected)
            {
                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;
                var Gizmos = nodeDesigner.CurrentSelection().OfType<NodeWrapper>();

                foreach (var item in Gizmos)
                {
                    double left = Canvas.GetLeft(item);
                    double top = Canvas.GetTop(item);

                    minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                    minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
                }
                
                double deltaHorizontal = Math.Max(-minLeft, NodeCanvasHelper.AdjustValueToGrid(e.HorizontalChange, nodeDesigner.GridOn, nodeDesigner.GridDelta));
                double deltaVertical = Math.Max(-minTop, NodeCanvasHelper.AdjustValueToGrid(e.VerticalChange, nodeDesigner.GridOn, nodeDesigner.GridDelta));

                foreach (var item in Gizmos)
                {
                    double left = Canvas.GetLeft(item);
                    double top = Canvas.GetTop(item);

                    if (double.IsNaN(left)) left = 0;
                    if (double.IsNaN(top)) top = 0;

                    Canvas.SetLeft(item, left + deltaHorizontal);
                    Canvas.SetTop(item, top + deltaVertical);
                    if (item.Node != null)
                    {
                        item.Node.Position.X = Canvas.GetLeft(item);
                        item.Node.Position.Y = Canvas.GetTop(item);
                    }
                }

                nodeDesigner.InvalidateMeasure();
                e.Handled = true;
            }
        }
    }
}
