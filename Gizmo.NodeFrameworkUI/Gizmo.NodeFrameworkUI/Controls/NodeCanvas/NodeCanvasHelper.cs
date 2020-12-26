using System;
using System.Windows;

namespace Gizmo.NodeFrameworkUI
{
    public class NodeCanvasHelper
    {
        public static double AdjustValueToGrid(double Value, bool GridOn, double GridDelta)
        {
            if (!GridOn)
                return Value;
            Value = Math.Round(Value / GridDelta, 0) * GridDelta;
            return Value;
        }

        public static Point AdjustPointToGrid(Point Point, bool GridOn, double GridDelta)
        {
            if (!GridOn)
                return Point;
            Point.X = Math.Round(Point.X / GridDelta, 0) * GridDelta;
            Point.Y = Math.Round(Point.Y / GridDelta, 0) * GridDelta;
            return Point;
        }

        public static void AdjustSizeToGrid(ref Size Size, bool GridOn, double GridDelta)
        {
            if (!GridOn)
                return;

            Size.Height = Math.Round(Size.Height / GridDelta, 0) * GridDelta;
            Size.Width = Math.Round(Size.Width / GridDelta, 0) * GridDelta;
        }

        public static Rect AdjustRectToGrid(Rect Rect, bool GridOn, double GridDelta)
        {
            if (!GridOn)
                return Rect;
            Point p1 = new Point(Rect.X, Rect.Y);
            Point p2 = new Point(Rect.X + Rect.Width, Rect.Y + Rect.Height);
            return new Rect(AdjustPointToGrid(p1, GridOn, GridDelta), AdjustPointToGrid(p2, GridOn, GridDelta));
        }
    }
}
