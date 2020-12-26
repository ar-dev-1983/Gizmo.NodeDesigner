using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Gizmo.NodeFrameworkUI
{
    public class NodeWrapperResizeThumb : Thumb
    {
        public NodeWrapperResizeThumb(): base()
        {
            DefaultStyleKey = typeof(NodeWrapperResizeThumb);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            base.DragDelta += new DragDeltaEventHandler(ResizeThumb_DragDelta);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(NodeWrapperResizeThumb), new FrameworkPropertyMetadata(new CornerRadius(0)));

        void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            NodeWrapper SelectedNode = DataContext as NodeWrapper;
            NodeCanvas nodeDesigner = VisualHelper.FindParent<NodeCanvas>(SelectedNode);

            if (SelectedNode != null && nodeDesigner != null && SelectedNode.IsSelected)
            {
                double dragDeltaVertical, dragDeltaHorizontal, scale;

                IEnumerable<NodeWrapper> SelectedNodes = nodeDesigner.CurrentSelection().OfType<NodeWrapper>();

                CalculateDragLimits(SelectedNodes, out double minLeft, out double minTop, out double minDeltaHorizontal, out double minDeltaVertical);

                foreach (var item in SelectedNodes)
                {
                    if (item != null && item.ParentId == Guid.Empty)
                    {
                        switch (base.VerticalAlignment)
                        {
                            case VerticalAlignment.Bottom:
                                dragDeltaVertical = Math.Min(-NodeCanvasHelper.AdjustValueToGrid(e.VerticalChange, nodeDesigner.GridOn, nodeDesigner.GridDelta), minDeltaVertical);
                                scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                                DragBottom(scale, item, nodeDesigner.SelectionService());
                                break;
                            case VerticalAlignment.Top:
                                double top = Canvas.GetTop(item);
                                dragDeltaVertical = Math.Min(Math.Max(-minTop, NodeCanvasHelper.AdjustValueToGrid(e.VerticalChange, nodeDesigner.GridOn, nodeDesigner.GridDelta)), minDeltaVertical);
                                scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                                DragTop(scale, item, nodeDesigner.SelectionService());
                                break;
                            default:
                                break;
                        }

                        switch (base.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                double left = Canvas.GetLeft(item);
                                dragDeltaHorizontal = Math.Min(Math.Max(-minLeft, NodeCanvasHelper.AdjustValueToGrid(e.HorizontalChange, nodeDesigner.GridOn, nodeDesigner.GridDelta)), minDeltaHorizontal);
                                scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                                DragLeft(scale, item, nodeDesigner.SelectionService());
                                break;
                            case HorizontalAlignment.Right:
                                dragDeltaHorizontal = Math.Min(-NodeCanvasHelper.AdjustValueToGrid(e.HorizontalChange, nodeDesigner.GridOn, nodeDesigner.GridDelta), minDeltaHorizontal);
                                scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                                DragRight(scale, item, nodeDesigner.SelectionService());
                                break;
                            default:
                                break;
                        }
                        if (item.Node != null)
                        {
                            item.Node.Position.X = Canvas.GetLeft(item);
                            item.Node.Position.Y = Canvas.GetTop(item);
                        }
                    }
                }
                e.Handled = true;
            }
        }

        private void DragLeft(double scale, NodeWrapper item, SelectionService selectionService)
        {
            IEnumerable<NodeWrapper> groupItems = selectionService.GetGroupMembers(item).Cast<NodeWrapper>();

            double groupLeft = Canvas.GetLeft(item) + item.ActualWidth;
            foreach (var groupItem in groupItems)
            {
                double groupItemLeft = Canvas.GetLeft(groupItem);
                double delta = (groupLeft - groupItemLeft) * (scale - 1);
                Canvas.SetLeft(groupItem, groupItemLeft - delta);
                groupItem.Width = groupItem.ActualWidth * scale;
            }
        }

        private void DragTop(double scale, NodeWrapper item, SelectionService selectionService)
        {
            IEnumerable<NodeWrapper> groupItems = selectionService.GetGroupMembers(item).Cast<NodeWrapper>();
            double groupBottom = Canvas.GetTop(item) + item.ActualHeight;
            foreach (var groupItem in groupItems)
            {
                double groupItemTop = Canvas.GetTop(groupItem);
                double delta = (groupBottom - groupItemTop) * (scale - 1);
                Canvas.SetTop(groupItem, groupItemTop - delta);
                groupItem.Height = groupItem.ActualHeight * scale;
            }
        }

        private void DragRight(double scale, NodeWrapper item, SelectionService selectionService)
        {
            IEnumerable<NodeWrapper> groupItems = selectionService.GetGroupMembers(item).Cast<NodeWrapper>();

            double groupLeft = Canvas.GetLeft(item);
            foreach (var groupItem in groupItems)
            {
                double groupItemLeft = Canvas.GetLeft(groupItem);
                double delta = (groupItemLeft - groupLeft) * (scale - 1);

                Canvas.SetLeft(groupItem, groupItemLeft + delta);
                groupItem.Width = groupItem.ActualWidth * scale;
            }
        }

        private void DragBottom(double scale, NodeWrapper item, SelectionService selectionService)
        {
            IEnumerable<NodeWrapper> groupItems = selectionService.GetGroupMembers(item).Cast<NodeWrapper>();
            double groupTop = Canvas.GetTop(item);
            foreach (var groupItem in groupItems)
            {
                double groupItemTop = Canvas.GetTop(groupItem);
                double delta = (groupItemTop - groupTop) * (scale - 1);

                Canvas.SetTop(groupItem, groupItemTop + delta);
                groupItem.Height = groupItem.ActualHeight * scale;
            }
        }

        private void CalculateDragLimits(IEnumerable<NodeWrapper> selectedItems, out double minLeft, out double minTop, out double minDeltaHorizontal, out double minDeltaVertical)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            foreach (var item in selectedItems)
            {
                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                minDeltaVertical = Math.Min(minDeltaVertical, item.ActualHeight - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.ActualWidth - item.MinWidth);
            }
        }
    }

}
