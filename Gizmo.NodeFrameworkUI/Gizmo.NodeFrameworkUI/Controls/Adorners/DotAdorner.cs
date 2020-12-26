using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{

    public class DotAdorner : Adorner
    {
        private Point Point;
        private readonly Pen drawingPen;

        public DotAdorner(NodeCanvas designer, Point point, Brush brush)
            : base(designer)
        {
            Point = point;
            drawingPen = new Pen(brush, 2)
            {
                LineJoin = PenLineJoin.Round
            };
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawRectangle(drawingPen.Brush, drawingPen, new Rect(Point.X - 2, Point.Y - 2, 4, 4));
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }
    }

}
