using Gizmo.NodeFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{
    public class LinkWrapperAdorner : Adorner
    {
        public LinkWrapperAdorner(NodeCanvas Designer, LinkWrapper link)
       : base(Designer)
        {
            nodeDesigner = Designer;
            adornerCanvas = new Canvas();
            visualChildren = new VisualCollection(this) { adornerCanvas };

            this.link = link;
            this.link.PropertyChanged += new PropertyChangedEventHandler(AnchorPositionChanged);

            InitializeDragThumbs();

            drawingPen = new Pen(Brushes.LightSlateGray, 1)
            {
                LineJoin = PenLineJoin.Round
            };

            Unloaded += new RoutedEventHandler(ConnectionAdorner_Unloaded);
        }

        void AnchorPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AnchorPositionSource"))
            {
                Canvas.SetLeft(sourceDragThumb, link.AnchorPositionSource.X);
                Canvas.SetTop(sourceDragThumb, link.AnchorPositionSource.Y);
            }

            if (e.PropertyName.Equals("AnchorPositionDestination"))
            {
                Canvas.SetLeft(destinationDragThumb, link.AnchorPositionDestination.X);
                Canvas.SetTop(destinationDragThumb, link.AnchorPositionDestination.Y);
            }
        }

        private NodeCanvas nodeDesigner;
        private Canvas adornerCanvas;
        private LinkWrapper link;
        private PathGeometry pathGeometry;
        private VariableWrapper fixConnector, dragConnector;
        private Thumb sourceDragThumb, destinationDragThumb;
        private Pen drawingPen;

        private NodeWrapper hitDesignerItem;
        private NodeWrapper HitGizmoContainer
        {
            get { return hitDesignerItem; }
            set
            {
                if (hitDesignerItem != value)
                {
                    if (hitDesignerItem != null)
                        hitDesignerItem.IsDragConnectionOver = false;

                    hitDesignerItem = value;

                    if (hitDesignerItem != null)
                        hitDesignerItem.IsDragConnectionOver = true;
                }
            }
        }

        private VariableWrapper hitConnector;
        private VariableWrapper HitConnector
        {
            get { return hitConnector; }
            set
            {
                if (hitConnector != value)
                {
                    hitConnector = value;
                }
            }
        }

        private VisualCollection visualChildren;
        protected override int VisualChildrenCount => visualChildren.Count;

        protected override Visual GetVisualChild(int index)
        {
            return visualChildren[index];
        }

        void ThumbDragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (HitConnector != null)
            {
                if (link != null)
                {
                    if (link.Source == fixConnector)
                        link.Destination = HitConnector;
                    else
                        link.Source = HitConnector;
                }
            }

            HitGizmoContainer = null;
            HitConnector = null;
            pathGeometry = null;
            InvalidateVisual();
        }

        void ThumbDragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            HitGizmoContainer = null;
            HitConnector = null;
            pathGeometry = null;
            Cursor = Cursors.Cross;
            if (sender == sourceDragThumb)
            {
                fixConnector = link.Destination;
                dragConnector = link.Source;
            }
            else if (sender == destinationDragThumb)
            {
                dragConnector = link.Destination;
                fixConnector = link.Source;
            }
        }

        void ThumbDragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point currentPosition = Mouse.GetPosition(this);
            HitTesting(currentPosition);
            pathGeometry = UpdatePathGeometry(NodeCanvasHelper.AdjustPointToGrid(currentPosition, nodeDesigner.GridOn, nodeDesigner.GridDelta));
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, drawingPen, pathGeometry);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            adornerCanvas.Arrange(new Rect(0, 0, nodeDesigner.ActualWidth, nodeDesigner.ActualHeight));
            return finalSize;
        }

        private void ConnectionAdorner_Unloaded(object sender, RoutedEventArgs e)
        {
            sourceDragThumb.DragDelta -= new DragDeltaEventHandler(ThumbDragThumb_DragDelta);
            sourceDragThumb.DragStarted -= new DragStartedEventHandler(ThumbDragThumb_DragStarted);
            sourceDragThumb.DragCompleted -= new DragCompletedEventHandler(ThumbDragThumb_DragCompleted);

            destinationDragThumb.DragDelta -= new DragDeltaEventHandler(ThumbDragThumb_DragDelta);
            destinationDragThumb.DragStarted -= new DragStartedEventHandler(ThumbDragThumb_DragStarted);
            destinationDragThumb.DragCompleted -= new DragCompletedEventHandler(ThumbDragThumb_DragCompleted);
        }

        private void InitializeDragThumbs()
        {
            Style dragThumbStyle = link.FindResource("ConnectionAdornerThumbStyle") as Style;

            sourceDragThumb = new Thumb();
            Canvas.SetLeft(sourceDragThumb, link.AnchorPositionSource.X);
            Canvas.SetTop(sourceDragThumb, link.AnchorPositionSource.Y);
            adornerCanvas.Children.Add(sourceDragThumb);
            if (dragThumbStyle != null)
                sourceDragThumb.Style = dragThumbStyle;

            sourceDragThumb.DragDelta += new DragDeltaEventHandler(ThumbDragThumb_DragDelta);
            sourceDragThumb.DragStarted += new DragStartedEventHandler(ThumbDragThumb_DragStarted);
            sourceDragThumb.DragCompleted += new DragCompletedEventHandler(ThumbDragThumb_DragCompleted);

            destinationDragThumb = new Thumb();
            Canvas.SetLeft(destinationDragThumb, link.AnchorPositionDestination.X);
            Canvas.SetTop(destinationDragThumb, link.AnchorPositionDestination.Y);
            adornerCanvas.Children.Add(destinationDragThumb);
            if (dragThumbStyle != null)
                destinationDragThumb.Style = dragThumbStyle;

            destinationDragThumb.DragDelta += new DragDeltaEventHandler(ThumbDragThumb_DragDelta);
            destinationDragThumb.DragStarted += new DragStartedEventHandler(ThumbDragThumb_DragStarted);
            destinationDragThumb.DragCompleted += new DragCompletedEventHandler(ThumbDragThumb_DragCompleted);
        }

        private PathGeometry UpdatePathGeometry(Point position)
        {
            PathGeometry geometry = new PathGeometry();

            VariableWrapperOrientation targetOrientation;
            if (HitConnector != null)
                targetOrientation = HitConnector.Orientation;
            else
                targetOrientation = dragConnector.Orientation;

            List<Point> LinkPoints = LinkWrapper.GetLink(fixConnector.GetInfo(), position, nodeDesigner.LinkStyle, targetOrientation);
            if (nodeDesigner.LinkStyle == LinkStyle.RoutePath)
            {
                LinkPoints.Insert(0, new Point(fixConnector.Position.X + 5, fixConnector.Position.Y));
                LinkPoints.Add(new Point(position.X - 5, position.Y));
            }

            if (LinkPoints.Count > 0)
            {
                PathFigure figure = new PathFigure
                {
                    StartPoint = LinkPoints[0]
                };
                LinkPoints.Remove(LinkPoints[0]);
                figure.Segments.Add(nodeDesigner.LinkStyle == LinkStyle.RoutePath || nodeDesigner.LinkStyle == LinkStyle.Simple ? new PolyLineSegment(LinkPoints, true) as PathSegment : new PolyBezierSegment(LinkPoints, true) as PathSegment);
                geometry.Figures.Add(figure);
            }
            return geometry;
        }

        private void HitTesting(Point hitPoint)
        {
            bool hitConnectorFlag = false;

            DependencyObject hitObject = nodeDesigner.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null && hitObject != fixConnector.ParentNodeWrapper && hitObject.GetType() != typeof(NodeCanvas))
            {
                if (hitObject is VariableWrapper)
                {
                    if (fixConnector.Variable.VariableType == VariableType.Input && (hitObject as VariableWrapper).Variable.VariableType == VariableType.Output)
                    {
                        //output can have multiple connections
                        HitConnector = hitObject as VariableWrapper;
                        hitConnectorFlag = true;
                    }
                    else if (fixConnector.Variable.VariableType == VariableType.Output && (hitObject as VariableWrapper).Variable.VariableType == VariableType.Input)
                    {
                        //input can have only one connection
                        HitConnector = hitObject as VariableWrapper;
                        if (HitConnector.Variable.IsConnected)
                            hitConnectorFlag = false;
                        else
                            hitConnectorFlag = true;
                    }
                }

                if (hitObject is NodeWrapper)
                {
                    HitGizmoContainer = hitObject as NodeWrapper;
                    if (!hitConnectorFlag)
                        HitConnector = null;
                    return;
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

            HitConnector = null;
            HitGizmoContainer = null;
        }
    }


}
