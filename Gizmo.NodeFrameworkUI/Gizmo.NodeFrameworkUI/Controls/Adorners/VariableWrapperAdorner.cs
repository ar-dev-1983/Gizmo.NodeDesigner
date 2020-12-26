using Gizmo.NodeFramework;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{

    public class VariableWrapperAdorner : Adorner
    {
        private PathGeometry pathGeometry;
        private NodeCanvas nodeDesignerCanvas;
        private VariableWrapper sourceConnector;
        private readonly Pen drawingPen;

        private NodeWrapper hitNode;
        private NodeWrapper HitNode
        {
            get { return hitNode; }
            set
            {
                if (hitNode != value)
                {
                    if (hitNode != null)
                        hitNode.IsDragConnectionOver = false;

                    hitNode = value;

                    if (hitNode != null)
                        hitNode.IsDragConnectionOver = true;
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

        public VariableWrapperAdorner(NodeCanvas designer, VariableWrapper Connector)
            : base(designer)
        {
            nodeDesignerCanvas = designer;
            sourceConnector = Connector;
            drawingPen = new Pen(sourceConnector.BorderBrush, 2)
            {
                LineJoin = PenLineJoin.Round
            };
            Cursor = Cursors.Cross;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (HitConnector != null)
            {
                VariableWrapper source = sourceConnector;
                VariableWrapper destination = HitConnector;

                if (!source.Variable.IsConnected && source.Variable.VariableType == VariableType.Input && destination.Variable.VariableType == VariableType.Output)
                {
                    nodeDesignerCanvas.AddLink(destination, source);
                }
                else if (source.Variable.VariableType == VariableType.Output && destination.Variable.VariableType == VariableType.Input && !destination.Variable.IsConnected)
                {
                    nodeDesignerCanvas.AddLink(source, destination);
                }
            }

            if (HitNode != null)
            {
                HitNode.IsDragConnectionOver = false;
            }

            if (IsMouseCaptured) ReleaseMouseCapture();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(nodeDesignerCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured) this.CaptureMouse();
                HitTesting(e.GetPosition(this));

                pathGeometry = GetPathGeometry(NodeCanvasHelper.AdjustPointToGrid(e.GetPosition(this), nodeDesignerCanvas.GridOn, nodeDesignerCanvas.GridDelta));
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, drawingPen, pathGeometry);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        private PathGeometry GetPathGeometry(Point position)
        {
            PathGeometry geometry = new PathGeometry();

            VariableWrapperOrientation targetOrientation;
            if (HitConnector != null)
                targetOrientation = HitConnector.Orientation;
            else
                targetOrientation = VariableWrapperOrientation.None;

            List<Point> LinkPoints = LinkWrapper.GetLink(sourceConnector.GetInfo(), position, nodeDesignerCanvas.LinkStyle, targetOrientation);

            if (nodeDesignerCanvas.LinkStyle == LinkStyle.RoutePath)
            {
                LinkPoints.Insert(0, new Point(sourceConnector.Position.X + 5, sourceConnector.Position.Y));
                LinkPoints.Add(new Point(position.X - 5, position.Y));
            }

            if (LinkPoints.Count > 0)
            {
                PathFigure figure = new PathFigure
                {
                    StartPoint = LinkPoints[0]
                };
                LinkPoints.Remove(LinkPoints[0]);
                figure.Segments.Add(nodeDesignerCanvas.LinkStyle == LinkStyle.RoutePath || nodeDesignerCanvas.LinkStyle == LinkStyle.Simple ? new PolyLineSegment(LinkPoints, true) as PathSegment : new PolyBezierSegment(LinkPoints, true) as PathSegment);
                geometry.Figures.Add(figure);
            }

            return geometry;
        }

        private void HitTesting(Point hitPoint)
        {
            bool hitConnectorFlag = false;

            DependencyObject hitObject = nodeDesignerCanvas.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null && hitObject != sourceConnector.ParentNodeWrapper && hitObject.GetType() != typeof(NodeCanvas))
            {
                if (hitObject is VariableWrapper)
                {
                    if (sourceConnector.Variable.VariableType == VariableType.Input && (hitObject as VariableWrapper).Variable.VariableType == VariableType.Output)
                    {
                        HitConnector = hitObject as VariableWrapper;
                        hitConnectorFlag = true;
                    }
                    else if (sourceConnector.Variable.VariableType == VariableType.Output && (hitObject as VariableWrapper).Variable.VariableType == VariableType.Input)
                    {
                        HitConnector = hitObject as VariableWrapper;
                        hitConnectorFlag = true;
                    }
                }
                if (hitObject is NodeWrapper)
                {
                    HitNode = hitObject as NodeWrapper;
                    if (!hitConnectorFlag)
                        HitConnector = null;
                    return;
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

            HitConnector = null;
            HitNode = null;
        }
    }

}
