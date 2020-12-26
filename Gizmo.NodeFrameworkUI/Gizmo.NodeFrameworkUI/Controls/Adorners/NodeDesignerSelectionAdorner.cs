using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{
    public class NodeDesignerSelectionAdorner : Adorner
    {
        private Point? startPoint;
        private Point? endPoint;
        private Pen rubberbandPen;

        private readonly NodeCanvas Designer;

        public NodeDesignerSelectionAdorner(NodeCanvas nodeDesigner, Point? dragStartPoint)
            : base(nodeDesigner)
        {
            Designer = nodeDesigner;
            startPoint = dragStartPoint;
            rubberbandPen = new Pen(Designer.GridStroke, 2)
            {
                DashStyle = new DashStyle(new double[] { 2 }, 1)
            };
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                    CaptureMouse();

                endPoint = Designer.GridOn ? NodeCanvasHelper.AdjustPointToGrid(e.GetPosition(this), Designer.GridOn, Designer.GridDelta) : e.GetPosition(this);
                UpdateSelection();
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured) ReleaseMouseCapture();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(Designer);
            if (adornerLayer != null)
                adornerLayer.Remove(this);

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (startPoint.HasValue && endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, rubberbandPen, new Rect(startPoint.Value, endPoint.Value));
        }

        private void UpdateSelection()
        {
            Designer.ClearSelection();

            Rect rubberBand = new Rect(startPoint.Value, endPoint.Value);
            foreach (Control item in Designer.Children)
            {
                Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                Rect itemBounds = item.TransformToAncestor(Designer).TransformBounds(itemRect);

                if (rubberBand.Contains(itemBounds))
                {
                    if (item is VariableWrapper)
                    {
                        //Designer.AddToSelection(item as ISelectable);
                    }
                    else if (item is NodeWrapper)
                    {
                        Designer.AddToSelection(item as NodeWrapper);
                    }
                    else if (item is LinkWrapper)
                    {
                        Designer.AddToSelection(item as LinkWrapper);
                    }
                }
            }
        }
    }

}
