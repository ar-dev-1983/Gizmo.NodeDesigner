using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.NodeFrameworkUI
{

    public partial class LinkWrapper
    {
        private const int margin = 30;
        internal static List<Point> GetLink(VariableWrapperInfo Source, VariableWrapperInfo Destination, LinkStyle Style)
        {
            List<Point> LinkPoints = new List<Point>();
            if (Style != LinkStyle.RoutePath)
            {
                var SourceRect = GetRectWithMargin(Source, margin);
                var DestinationRect = GetRectWithMargin(Destination, margin);

                var SourceOffset = GetOffsetPoint(Source, SourceRect);
                var DestinationOffset = GetOffsetPoint(Destination, DestinationRect);
                LinkPoints = new List<Point>
                {
                    Source.Position,
                    SourceOffset,
                    DestinationOffset,
                    Destination.Position
                };
            }
            else
            {
                Rect SourceRect = GetRectWithMargin(Source, margin);
                Rect DestinationRect = GetRectWithMargin(Destination, margin);

                Point SourceOffset = GetOffsetPoint(Source, SourceRect);
                Point DestinationOffset = GetOffsetPoint(Destination, DestinationRect);

                LinkPoints.Add(SourceOffset);
                Point CurrentPoint = SourceOffset;

                if (!DestinationRect.Contains(CurrentPoint) && !SourceRect.Contains(DestinationOffset))
                {
                    while (true)
                    {

                        if (IsPointVisible(CurrentPoint, DestinationOffset, new Rect[] { SourceRect, DestinationRect }))
                        {
                            LinkPoints.Add(DestinationOffset);
                            CurrentPoint = DestinationOffset;
                            break;
                        }

                        Point DestinationNeighbour = GetNearestVisibleNeighborDestination(CurrentPoint, DestinationOffset, Destination, SourceRect, DestinationRect);
                        if (!double.IsNaN(DestinationNeighbour.X))
                        {
                            LinkPoints.Add(DestinationNeighbour);
                            LinkPoints.Add(DestinationOffset);
                            CurrentPoint = DestinationOffset;
                            break;
                        }

                        if (CurrentPoint == SourceOffset)
                        {
                            bool ResultFlag;
                            Point SourceNeighbour = GetNearestNeighborSource(Source, DestinationOffset, SourceRect, DestinationRect, out ResultFlag);
                            LinkPoints.Add(SourceNeighbour);
                            CurrentPoint = SourceNeighbour;

                            if (!IsRectVisible(CurrentPoint, DestinationRect, new Rect[] { SourceRect }))
                            {
                                Point Point1, Point2;
                                GetOppositeCorners(Source.Orientation, SourceRect, out Point1, out Point2);
                                if (ResultFlag)
                                {
                                    LinkPoints.Add(Point1);
                                    CurrentPoint = Point1;
                                }
                                else
                                {
                                    LinkPoints.Add(Point2);
                                    CurrentPoint = Point2;
                                }
                                if (!IsRectVisible(CurrentPoint, DestinationRect, new Rect[] { SourceRect }))
                                {
                                    if (ResultFlag)
                                    {
                                        LinkPoints.Add(Point2);
                                        CurrentPoint = Point2;
                                    }
                                    else
                                    {
                                        LinkPoints.Add(Point1);
                                        CurrentPoint = Point1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Point NeighbourCorner1, NeighbourCorner2;
                            Point OppostieCorner1, OppostieCorner2;
                            GetNeighborCorners(Destination.Orientation, DestinationRect, out OppostieCorner1, out OppostieCorner2);
                            GetOppositeCorners(Destination.Orientation, DestinationRect, out NeighbourCorner1, out NeighbourCorner2);

                            bool n1Visible = IsPointVisible(CurrentPoint, NeighbourCorner1, new Rect[] { SourceRect, DestinationRect });
                            bool n2Visible = IsPointVisible(CurrentPoint, NeighbourCorner2, new Rect[] { SourceRect, DestinationRect });

                            if (n1Visible && n2Visible)
                            {
                                if (SourceRect.Contains(NeighbourCorner1))
                                {
                                    LinkPoints.Add(NeighbourCorner2);
                                    if (SourceRect.Contains(OppostieCorner2))
                                    {
                                        LinkPoints.Add(NeighbourCorner1);
                                        LinkPoints.Add(OppostieCorner1);
                                    }
                                    else
                                        LinkPoints.Add(OppostieCorner2);

                                    LinkPoints.Add(DestinationOffset);
                                    CurrentPoint = DestinationOffset;
                                    break;
                                }

                                if (SourceRect.Contains(NeighbourCorner2))
                                {
                                    LinkPoints.Add(NeighbourCorner1);
                                    if (SourceRect.Contains(OppostieCorner1))
                                    {
                                        LinkPoints.Add(NeighbourCorner2);
                                        LinkPoints.Add(OppostieCorner2);
                                    }
                                    else
                                        LinkPoints.Add(OppostieCorner1);

                                    LinkPoints.Add(DestinationOffset);
                                    CurrentPoint = DestinationOffset;
                                    break;
                                }

                                if ((Distance(NeighbourCorner1, DestinationOffset) <= Distance(NeighbourCorner2, DestinationOffset)))
                                {
                                    LinkPoints.Add(NeighbourCorner1);
                                    if (SourceRect.Contains(OppostieCorner1))
                                    {
                                        LinkPoints.Add(NeighbourCorner2);
                                        LinkPoints.Add(OppostieCorner2);
                                    }
                                    else
                                        LinkPoints.Add(OppostieCorner1);
                                    LinkPoints.Add(DestinationOffset);
                                    CurrentPoint = DestinationOffset;
                                    break;
                                }
                                else
                                {
                                    LinkPoints.Add(NeighbourCorner2);
                                    if (SourceRect.Contains(OppostieCorner2))
                                    {
                                        LinkPoints.Add(NeighbourCorner1);
                                        LinkPoints.Add(OppostieCorner1);
                                    }
                                    else
                                        LinkPoints.Add(OppostieCorner2);
                                    LinkPoints.Add(DestinationOffset);
                                    CurrentPoint = DestinationOffset;
                                    break;
                                }
                            }
                            else if (n1Visible)
                            {
                                LinkPoints.Add(NeighbourCorner1);
                                if (SourceRect.Contains(OppostieCorner1))
                                {
                                    LinkPoints.Add(NeighbourCorner2);
                                    LinkPoints.Add(OppostieCorner2);
                                }
                                else
                                    LinkPoints.Add(OppostieCorner1);
                                LinkPoints.Add(DestinationOffset);
                                CurrentPoint = DestinationOffset;
                                break;
                            }
                            else
                            {
                                LinkPoints.Add(NeighbourCorner2);
                                if (SourceRect.Contains(OppostieCorner2))
                                {
                                    LinkPoints.Add(NeighbourCorner1);
                                    LinkPoints.Add(OppostieCorner1);
                                }
                                else
                                    LinkPoints.Add(OppostieCorner2);
                                LinkPoints.Add(DestinationOffset);
                                CurrentPoint = DestinationOffset;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    LinkPoints.Add(DestinationOffset);
                }
                LinkPoints = RefineGeometry(LinkPoints, new Rect[] { SourceRect, DestinationRect }, Source.Orientation, Destination.Orientation, Source.Position, Destination.Position);
            }
            return LinkPoints;
        }

        internal static List<Point> GetLink(VariableWrapperInfo Source, Point Destination, LinkStyle Style, VariableWrapperOrientation DesiredOrientation)
        {
            List<Point> LinkPoints = new List<Point>();
            if (Style != LinkStyle.RoutePath)
            {
                var cinfo = new VariableWrapperInfo() { Left = Destination.X - margin / 3, Top = Destination.Y - margin / 3, Size = new Size(2 * margin / 3, 2 * margin / 3), Orientation = GetOpositeOrientation(Source.Orientation), Position = Destination };
                LinkPoints = new List<Point>
                {
                    Source.Position,
                    GetOffsetPoint(Source, GetRectWithMargin(Source, margin)),
                    GetOffsetPoint(cinfo, GetRectWithMargin(cinfo, margin/3)),
                    Destination
                };
            }
            else
            {
                var SourceRect = GetRectWithMargin(Source, margin);
                var SourceOffset = GetOffsetPoint(Source, SourceRect);
                var DestinationOffset = Destination;

                LinkPoints.Add(SourceOffset);
                Point CurrentPoint = SourceOffset;

                if (!SourceRect.Contains(DestinationOffset))
                {
                    while (true)
                    {
                        if (IsPointVisible(CurrentPoint, DestinationOffset, new Rect[] { SourceRect }))
                        {
                            LinkPoints.Add(DestinationOffset);
                            break;
                        }

                        bool sideFlag;
                        Point Neighbor = GetNearestNeighborSource(Source, DestinationOffset, SourceRect, out sideFlag);
                        LinkPoints.Add(Neighbor);
                        CurrentPoint = Neighbor;

                        if (IsPointVisible(CurrentPoint, DestinationOffset, new Rect[] { SourceRect }))
                        {
                            LinkPoints.Add(DestinationOffset);
                            break;
                        }
                        else
                        {
                            Point Point1, Point2;
                            GetOppositeCorners(Source.Orientation, SourceRect, out Point1, out Point2);
                            if (sideFlag)
                                LinkPoints.Add(Point1);
                            else
                                LinkPoints.Add(Point2);

                            LinkPoints.Add(DestinationOffset);
                            break;
                        }
                    }
                }
                else
                {
                    LinkPoints.Add(DestinationOffset);
                }
                if (DesiredOrientation != VariableWrapperOrientation.None)
                    LinkPoints = RefineGeometry(LinkPoints, new Rect[] { SourceRect }, Source.Orientation, DesiredOrientation, Source.Position, Destination);
                else
                    LinkPoints = RefineGeometry(LinkPoints, new Rect[] { SourceRect }, Source.Orientation, GetOpositeOrientation(Source.Orientation), Source.Position, Destination);

            }
            return LinkPoints;
        }

        private static List<Point> RefineGeometry(List<Point> LinePoints, Rect[] Rectangles, VariableWrapperOrientation SourceOrientation, VariableWrapperOrientation DestinationkOrientation, Point Source, Point Destination)
        {
            List<Point> Result = new List<Point>();
            int CutPoint = 0;

            for (int i = 0; i < LinePoints.Count; i++)
            {
                if (i >= CutPoint)
                {
                    for (int k = LinePoints.Count - 1; k > i; k--)
                    {
                        if (IsPointVisible(LinePoints[i], LinePoints[k], Rectangles))
                        {
                            CutPoint = k;
                            break;
                        }
                    }
                    Result.Add(LinePoints[i]);
                }
            }

            for (int j = 0; j < Result.Count - 1; j++)
            {
                if (Result[j].X != Result[j + 1].X && Result[j].Y != Result[j + 1].Y)
                {
                    VariableWrapperOrientation orientationFrom;
                    VariableWrapperOrientation orientationTo;

                    if (j == 0)
                        orientationFrom = SourceOrientation;
                    else
                        orientationFrom = GetOrientation(Result[j], Result[j - 1]);

                    if (j == Result.Count - 2)
                        orientationTo = DestinationkOrientation;
                    else
                        orientationTo = GetOrientation(Result[j + 1], Result[j + 2]);


                    if ((orientationFrom == VariableWrapperOrientation.Left || orientationFrom == VariableWrapperOrientation.Right) &&
                        (orientationTo == VariableWrapperOrientation.Left || orientationTo == VariableWrapperOrientation.Right))
                    {
                        double centerX = Math.Min(Result[j].X, Result[j + 1].X) + Math.Abs(Result[j].X - Result[j + 1].X) / 2;
                        Result.Insert(j + 1, new Point(centerX, Result[j].Y));
                        Result.Insert(j + 2, new Point(centerX, Result[j + 2].Y));
                        if (Result.Count - 1 > j + 3)
                            Result.RemoveAt(j + 3);
                        return Result;
                    }

                    if ((orientationFrom == VariableWrapperOrientation.Top || orientationFrom == VariableWrapperOrientation.Bottom) &&
                        (orientationTo == VariableWrapperOrientation.Top || orientationTo == VariableWrapperOrientation.Bottom))
                    {
                        double centerY = Math.Min(Result[j].Y, Result[j + 1].Y) + Math.Abs(Result[j].Y - Result[j + 1].Y) / 2;
                        Result.Insert(j + 1, new Point(Result[j].X, centerY));
                        Result.Insert(j + 2, new Point(Result[j + 2].X, centerY));
                        if (Result.Count - 1 > j + 3)
                            Result.RemoveAt(j + 3);
                        return Result;
                    }

                    if ((orientationFrom == VariableWrapperOrientation.Left || orientationFrom == VariableWrapperOrientation.Right) &&
                        (orientationTo == VariableWrapperOrientation.Top || orientationTo == VariableWrapperOrientation.Bottom))
                    {
                        Result.Insert(j + 1, new Point(Result[j + 1].X, Result[j].Y));
                        return Result;
                    }

                    if ((orientationFrom == VariableWrapperOrientation.Top || orientationFrom == VariableWrapperOrientation.Bottom) &&
                        (orientationTo == VariableWrapperOrientation.Left || orientationTo == VariableWrapperOrientation.Right))
                    {
                        Result.Insert(j + 1, new Point(Result[j].X, Result[j + 1].Y));
                        return Result;
                    }
                }
            }
            return Result;
        }

        private static VariableWrapperOrientation GetOrientation(Point p1, Point p2)
        {
            if (p1.X == p2.X)
            {
                if (p1.Y >= p2.Y)
                    return VariableWrapperOrientation.Bottom;
                else
                    return VariableWrapperOrientation.Top;
            }
            if (p1.Y == p2.Y)
            {
                if (p1.X >= p2.X)
                    return VariableWrapperOrientation.Right;
                else
                    return VariableWrapperOrientation.Left;
            }
            throw new Exception("Unknown orientation");
        }

        private static Orientation GetOrientation(VariableWrapperOrientation orientation)
        {
            switch (orientation)
            {
                case VariableWrapperOrientation.Left:
                    return Orientation.Horizontal;
                case VariableWrapperOrientation.Top:
                    return Orientation.Vertical;
                case VariableWrapperOrientation.Right:
                    return Orientation.Horizontal;
                case VariableWrapperOrientation.Bottom:
                    return Orientation.Vertical;
                default:
                    throw new Exception("Unknown Orientation");
            }
        }

        private static VariableWrapperOrientation GetOpositeOrientation(VariableWrapperOrientation orientation)
        {
            switch (orientation)
            {
                case VariableWrapperOrientation.Left:
                    return VariableWrapperOrientation.Right;
                case VariableWrapperOrientation.Top:
                    return VariableWrapperOrientation.Bottom;
                case VariableWrapperOrientation.Right:
                    return VariableWrapperOrientation.Left;
                case VariableWrapperOrientation.Bottom:
                    return VariableWrapperOrientation.Top;
                default:
                    throw new Exception("Unknown Orientation");
            }
        }

        private static double Distance(Point p1, Point p2)
        {
            return Point.Subtract(p1, p2).Length;
        }

        private static Rect GetRectWithMargin(VariableWrapperInfo cpThumb, double margin)
        {
            Rect rect = new Rect(cpThumb.Left, cpThumb.Top, cpThumb.Size.Width, cpThumb.Size.Height);
            rect.Inflate(margin, margin / 3);
            return rect;
        }

        private static Point GetOffsetPoint(VariableWrapperInfo cpThumb, Rect rect)
        {
            Point offsetPoint = new Point();

            switch (cpThumb.Orientation)
            {
                case VariableWrapperOrientation.Left:
                    offsetPoint = new Point(rect.Left, cpThumb.Position.Y);
                    break;
                case VariableWrapperOrientation.Right:
                    offsetPoint = new Point(rect.Right, cpThumb.Position.Y);
                    break;
                default:
                    break;
            }

            return offsetPoint;
        }

        private static Point GetOffsetPoint(Point point, Rect rect, VariableWrapperOrientation orientation)
        {
            Point offsetPoint = new Point();

            switch (orientation)
            {
                case VariableWrapperOrientation.Left:
                    offsetPoint = new Point(rect.Left, point.Y);
                    break;
                case VariableWrapperOrientation.Top:
                    offsetPoint = new Point(point.X, rect.Top);
                    break;
                case VariableWrapperOrientation.Right:
                    offsetPoint = new Point(rect.Right, point.Y);
                    break;
                case VariableWrapperOrientation.Bottom:
                    offsetPoint = new Point(point.X, rect.Bottom);
                    break;
                default:
                    break;
            }

            return offsetPoint;
        }

        private static bool IsPointVisible(Point FromPoint, Point TargetPoint, Rect[] Rects)
        {
            foreach (var node in Rects)
            {
                if (IsRectangleIntersectsLine(node, FromPoint, TargetPoint))
                    return false;
            }
            return true;
        }

        private static bool IsRectangleIntersectsLine(Rect Rect, Point StartPoint, Point EndPoint)
        {
            Rect.Inflate(-1, -1);
            return Rect.IntersectsWith(new Rect(StartPoint, EndPoint));
        }

        private static bool IsRectVisible(Point SourcePoint, Rect DestinationRect, Rect[] Rectangles)
        {
            if (IsPointVisible(SourcePoint, DestinationRect.TopLeft, Rectangles) ||
                IsPointVisible(SourcePoint, DestinationRect.TopRight, Rectangles) ||
                IsPointVisible(SourcePoint, DestinationRect.BottomLeft, Rectangles) ||
                IsPointVisible(SourcePoint, DestinationRect.BottomRight, Rectangles))
                return true;

            return false;
        }

        private static Point GetNearestNeighborSource(VariableWrapperInfo Source, Point EndPoint, Rect SourceRect, out bool Result)
        {
            Point Corner1, Corner2;
            GetNeighborCorners(Source.Orientation, SourceRect, out Corner1, out Corner2);

            if ((Distance(Corner1, EndPoint) <= Distance(Corner2, EndPoint)))
            {
                Result = true;
                return Corner1;
            }
            else
            {
                Result = false;
                return Corner2;
            }
        }

        private static Point GetNearestNeighborSource(VariableWrapperInfo Source, Point EndPoint, Rect SourceRect, Rect DestinationRect, out bool Result)
        {
            Point Corner1, Corner2;
            GetNeighborCorners(Source.Orientation, SourceRect, out Corner1, out Corner2);

            if (DestinationRect.Contains(Corner1))
            {
                Result = false;
                return Corner2;
            }

            if (DestinationRect.Contains(Corner2))
            {
                Result = true;
                return Corner1;
            }

            if ((Distance(Corner1, EndPoint) <= Distance(Corner2, EndPoint)))
            {
                Result = true;
                return Corner1;
            }
            else
            {
                Result = false;
                return Corner2;
            }
        }

        private static Point GetNearestVisibleNeighborDestination(Point CurrentPoint, Point EndPoint, VariableWrapperInfo Destination, Rect SourceRect, Rect DestinationRect)
        {
            Point Corner1, Corner2;
            GetNeighborCorners(Destination.Orientation, DestinationRect, out Corner1, out Corner2);

            bool Corner1Visible = IsPointVisible(CurrentPoint, Corner1, new Rect[] { SourceRect, DestinationRect });
            bool Corner2Visible = IsPointVisible(CurrentPoint, Corner2, new Rect[] { SourceRect, DestinationRect });

            if (Corner1Visible)
            {
                if (Corner2Visible)
                {
                    if (DestinationRect.Contains(Corner1))
                    {
                        return Corner2;
                    }

                    if (DestinationRect.Contains(Corner2))
                    {
                        return Corner1;
                    }

                    if (Distance(Corner1, EndPoint) <= Distance(Corner2, EndPoint))
                    {
                        return Corner1;
                    }
                    else
                    {
                        return Corner2;
                    }
                }
                else
                {
                    return Corner1;
                }
            }
            else
            {
                if (Corner2Visible)
                {
                    return Corner2;
                }
                else
                {
                    return new Point(double.NaN, double.NaN);
                }
            }
        }

        private static void GetNeighborCorners(VariableWrapperOrientation Orientation, Rect Rect, out Point Point1, out Point Point2)
        {
            switch (Orientation)
            {
                case VariableWrapperOrientation.Left:
                    Point1 = Rect.TopLeft; Point2 = Rect.BottomLeft;
                    break;
                case VariableWrapperOrientation.Right:
                    Point1 = Rect.TopRight; Point2 = Rect.BottomRight;
                    break;
                case VariableWrapperOrientation.Top:
                    Point1 = Rect.TopLeft; Point2 = Rect.TopRight;
                    break;
                case VariableWrapperOrientation.Bottom:
                    Point1 = Rect.BottomLeft; Point2 = Rect.BottomRight;
                    break;

                default:
                    throw new Exception("No neighour corners found.");
            }
        }

        private static void GetOppositeCorners(VariableWrapperOrientation Orientation, Rect Rect, out Point Point1, out Point Point2)
        {
            switch (Orientation)
            {
                case VariableWrapperOrientation.Left:
                    Point1 = Rect.TopRight; Point2 = Rect.BottomRight;
                    break;
                case VariableWrapperOrientation.Right:
                    Point1 = Rect.TopLeft; Point2 = Rect.BottomLeft;
                    break;
                case VariableWrapperOrientation.Top:
                    Point1 = Rect.BottomLeft; Point2 = Rect.BottomRight;
                    break;
                case VariableWrapperOrientation.Bottom:
                    Point1 = Rect.TopLeft; Point2 = Rect.TopRight;
                    break;
                default:
                    throw new Exception("No opposite corners found!");
            }
        }
    }
}
