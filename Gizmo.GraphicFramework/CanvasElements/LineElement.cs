using Gizmo.GraphicFramework.Helpers;
using System;
using System.Windows;
using System.Windows.Media;

namespace Gizmo.GraphicFramework.CanvasElements
{
    public class LineElement : BaseCanvasElement
    {
        static LineElement()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LineElement), new FrameworkPropertyMetadata(typeof(LineElement)));
        }

        public LineElement(Guid id)
        {
            Id = id;
            ParentId = Guid.NewGuid();
            Loaded += new RoutedEventHandler(LineElement_Loaded);
        }
        public LineElement(Guid id, Point startPoint, Point endPoint, bool startArrow, bool endArrow, int startArrowType, int endArrowType, double lineWidth, int lineStyle, SolidColorBrush lineBrush, double rotation)
        {
            Id = id;
            ParentId = Guid.NewGuid();
            StartPoint = startPoint;
            EndPoint = endPoint;
            CenterPoint = new Point((StartPoint.Value.X + EndPoint.Value.X) / 2, (StartPoint.Value.Y + EndPoint.Value.Y) / 2);
            StartArrow = startArrow;
            EndArrow = endArrow;
            LineDashStyle = CanvasElementHelper.GetDashStyle(lineStyle);
            StartArrowType = startArrowType;
            EndArrowType = endArrowType;
            Rotation = rotation;
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;
            LineBrush = lineBrush;
            FillBrush = lineBrush;
            Thickness = lineWidth;
            Loaded += new RoutedEventHandler(LineElement_Loaded);
        }
        public LineElement() : this(Guid.NewGuid())
        {
            ParentId = Guid.NewGuid();
            Loaded += new RoutedEventHandler(LineElement_Loaded);
        }

        private void LineElement_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public bool StartArrow
        {
            get => (bool)GetValue(StartArrowProperty);
            set => SetValue(StartArrowProperty, value);
        }
        public bool EndArrow
        {
            get => (bool)GetValue(EndArrowProperty);
            set => SetValue(EndArrowProperty, value);
        }
        public int StartArrowType
        {
            get => (int)GetValue(StartArrowTypeProperty);
            set => SetValue(StartArrowTypeProperty, value);
        }
        public int EndArrowType
        {
            get => (int)GetValue(EndArrowTypeProperty);
            set => SetValue(EndArrowTypeProperty, value);
        }

        public static readonly DependencyProperty StartArrowProperty = DependencyProperty.Register("StartArrow", typeof(bool), typeof(LineElement), new UIPropertyMetadata(false));
        public static readonly DependencyProperty EndArrowProperty = DependencyProperty.Register("EndArrow", typeof(bool), typeof(LineElement), new UIPropertyMetadata(false));
        public static readonly DependencyProperty StartArrowTypeProperty = DependencyProperty.Register("StartArrowType", typeof(int), typeof(LineElement), new UIPropertyMetadata(0));
        public static readonly DependencyProperty EndArrowTypeProperty = DependencyProperty.Register("EndArrowType", typeof(int), typeof(LineElement), new UIPropertyMetadata(0));

        public Geometry GetDefGeometry()
        {
            return DefiningGeometry;
        }
        protected override Geometry DefiningGeometry => new GeometryGroup
        {
            FillRule = FillRule.EvenOdd,
            Children = { new LineGeometry() { StartPoint = StartPoint.Value, EndPoint = EndPoint.Value } }
        };
        public void UpdateRender()
        {
            InvalidateArrange();
            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (Rotation != 0)
            {
                dc.PushTransform(new RotateTransform(Rotation, CenterPoint.Value.X, CenterPoint.Value.Y));
                dc.DrawGeometry(Brushes.Transparent, new Pen(LineBrush, Thickness) { DashStyle = LineDashStyle }, DefiningGeometry);

                if (EndArrow)
                {
                    if (EndArrowType == 1)
                    {
                        double theta = Math.Atan2(StartPoint.Value.Y - EndPoint.Value.Y, StartPoint.Value.X - EndPoint.Value.X);
                        double sint = Math.Sin(theta);
                        double cost = Math.Cos(theta);
                        var Geometry = new GeometryGroup()
                        {
                            Children ={new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                                    {
                                        new PathFigure()
                                        {
                                            StartPoint = new Point( EndPoint.Value.X + ((Thickness + 7) * cost + (Thickness + 3) * sint),
                                                                    EndPoint.Value.Y - ((Thickness + 3) * cost - (Thickness + 7) * sint)),
                                            Segments = {
                                                new LineSegment() { Point = EndPoint.Value },
                                                new LineSegment() { Point = new Point( EndPoint.Value.X + (((Thickness + 7) * cost) - ((Thickness + 3) * sint)),
                                                                                        EndPoint.Value.Y + (((Thickness + 7) * sint) + ((Thickness + 3) * cost))) } }
                                        }
                                    }
                        }
                    }
                        };
                        dc.DrawGeometry(Brushes.Transparent, new Pen(LineBrush, Thickness) { DashStyle = DashStyles.Solid, EndLineCap = PenLineCap.Round, StartLineCap = PenLineCap.Round }, Geometry);
                    }
                    else if (EndArrowType == 2)
                    {
                        double theta = Math.Atan2(StartPoint.Value.Y - EndPoint.Value.Y, StartPoint.Value.X - EndPoint.Value.X);
                        double sint = Math.Sin(theta);
                        double cost = Math.Cos(theta);
                        var Geometry = new GeometryGroup()
                        {
                            Children ={new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                                    {
                                        new PathFigure()
                                        {
                                            StartPoint = new Point( EndPoint.Value.X + ((Thickness + 7) * cost + (Thickness + 3) * sint),
                                                                    EndPoint.Value.Y - ((Thickness + 3) * cost - (Thickness + 7) * sint)),
                                                IsClosed=true,
                                            Segments = {
                                                new LineSegment() { Point = EndPoint.Value },
                                                new LineSegment() { Point = new Point( EndPoint.Value.X + (((Thickness + 7) * cost) - ((Thickness + 3) * sint)),
                                                                                        EndPoint.Value.Y + (((Thickness + 7) * sint) + ((Thickness + 3) * cost))) } }
                                        }
                                    }
                        }
                    }
                        };
                        dc.DrawGeometry(LineBrush, new Pen(LineBrush, Thickness) { DashStyle = DashStyles.Solid, EndLineCap = PenLineCap.Round, StartLineCap = PenLineCap.Round }, Geometry);
                    }
                    else if (EndArrowType == 3)
                    {
                        Point pt = EndPoint.Value;
                        pt.X -= 3 * Thickness;
                        pt.Y -= 3 * Thickness;
                        dc.DrawGeometry(LineBrush, new Pen(LineBrush, Thickness), new RectangleGeometry(new Rect(pt, new Size(6 * Thickness, 6 * Thickness)), double.MaxValue, double.MaxValue));
                    }
                }
                if (StartArrow)
                {
                    if (StartArrowType == 1)
                    {
                        double theta = Math.Atan2(StartPoint.Value.Y - EndPoint.Value.Y, StartPoint.Value.X - EndPoint.Value.X);
                        double sint = Math.Sin(theta);
                        double cost = Math.Cos(theta);
                        var Geometry = new GeometryGroup()
                        {
                            Children ={new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                                    {
                                       new PathFigure()
                                            {
                                                StartPoint = new Point( StartPoint.Value.X - ((Thickness + 7) * cost + (Thickness + 3) * sint),
                                                                        StartPoint.Value.Y + ((Thickness + 3) * cost - (Thickness + 7) * sint)),
                                                Segments = {
                                                    new LineSegment() { Point = StartPoint.Value },
                                                    new LineSegment() { Point = new Point( StartPoint.Value.X - (((Thickness + 7) * cost) - ((Thickness + 3) * sint)),
                                                                                            StartPoint.Value.Y - (((Thickness + 7) * sint) + ((Thickness + 3) * cost))) } }
                                            }
                                    }
                        }
                    }
                        };
                        dc.DrawGeometry(Brushes.Transparent, new Pen(LineBrush, Thickness) { DashStyle = DashStyles.Solid, EndLineCap = PenLineCap.Round, StartLineCap = PenLineCap.Round }, Geometry);
                    }
                    else if (StartArrowType == 2)
                    {
                        double theta = Math.Atan2(StartPoint.Value.Y - EndPoint.Value.Y, StartPoint.Value.X - EndPoint.Value.X);
                        double sint = Math.Sin(theta);
                        double cost = Math.Cos(theta);
                        var Geometry = new GeometryGroup()
                        {
                            Children ={new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                                    {
                                       new PathFigure()
                                            {
                                                StartPoint = new Point( StartPoint.Value.X - ((Thickness + 7) * cost + (Thickness + 3) * sint),
                                                                        StartPoint.Value.Y + ((Thickness + 3) * cost - (Thickness + 7) * sint)),
                                                IsClosed=true,
                                                Segments = {
                                                    new LineSegment() { Point = StartPoint.Value },
                                                    new LineSegment() { Point = new Point( StartPoint.Value.X - (((Thickness + 7) * cost) - ((Thickness + 3) * sint)),
                                                                                            StartPoint.Value.Y - (((Thickness + 7) * sint) + ((Thickness + 3) * cost))) } }
                                            }
                                    }
                        }
                    }
                        };
                        dc.DrawGeometry(LineBrush, new Pen(LineBrush, Thickness) { DashStyle = DashStyles.Solid, EndLineCap = PenLineCap.Round, StartLineCap = PenLineCap.Round }, Geometry);
                    }
                    else if (StartArrowType == 3)
                    {
                        Point pt = StartPoint.Value;
                        pt.X -= 3 * Thickness;
                        pt.Y -= 3 * Thickness;
                        dc.DrawGeometry(LineBrush, new Pen(LineBrush, Thickness), new RectangleGeometry(new Rect(pt, new Size(6 * Thickness, 6 * Thickness)), double.MaxValue, double.MaxValue));
                    }
                }
                dc.Pop();
            }
            else
            {
                dc.DrawGeometry(Brushes.Transparent, new Pen(LineBrush, Thickness) { DashStyle = LineDashStyle }, DefiningGeometry);

                if (EndArrow)
                {
                    if (EndArrowType == 1)
                    {
                        double theta = Math.Atan2(StartPoint.Value.Y - EndPoint.Value.Y, StartPoint.Value.X - EndPoint.Value.X);
                        double sint = Math.Sin(theta);
                        double cost = Math.Cos(theta);
                        var Geometry = new GeometryGroup()
                        {
                            Children ={new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                                    {
                                        new PathFigure()
                                        {
                                            StartPoint = new Point( EndPoint.Value.X + ((Thickness + 7) * cost + (Thickness + 3) * sint),
                                                                    EndPoint.Value.Y - ((Thickness + 3) * cost - (Thickness + 7) * sint)),
                                            Segments = {
                                                new LineSegment() { Point = EndPoint.Value },
                                                new LineSegment() { Point = new Point( EndPoint.Value.X + (((Thickness + 7) * cost) - ((Thickness + 3) * sint)),
                                                                                        EndPoint.Value.Y + (((Thickness + 7) * sint) + ((Thickness + 3) * cost))) } }
                                        }
                                    }
                        }
                    }
                        };
                        dc.DrawGeometry(Brushes.Transparent, new Pen(LineBrush, Thickness) { DashStyle = DashStyles.Solid, EndLineCap = PenLineCap.Round, StartLineCap = PenLineCap.Round }, Geometry);
                    }
                    else if (EndArrowType == 2)
                    {
                        double theta = Math.Atan2(StartPoint.Value.Y - EndPoint.Value.Y, StartPoint.Value.X - EndPoint.Value.X);
                        double sint = Math.Sin(theta);
                        double cost = Math.Cos(theta);
                        var Geometry = new GeometryGroup()
                        {
                            Children ={new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                                    {
                                        new PathFigure()
                                        {
                                            StartPoint = new Point( EndPoint.Value.X + ((Thickness + 7) * cost + (Thickness + 3) * sint),
                                                                    EndPoint.Value.Y - ((Thickness + 3) * cost - (Thickness + 7) * sint)),
                                                IsClosed=true,
                                            Segments = {
                                                new LineSegment() { Point = EndPoint.Value },
                                                new LineSegment() { Point = new Point( EndPoint.Value.X + (((Thickness + 7) * cost) - ((Thickness + 3) * sint)),
                                                                                        EndPoint.Value.Y + (((Thickness + 7) * sint) + ((Thickness + 3) * cost))) } }
                                        }
                                    }
                        }
                    }
                        };
                        dc.DrawGeometry(LineBrush, new Pen(LineBrush, Thickness) { DashStyle = DashStyles.Solid, EndLineCap = PenLineCap.Round, StartLineCap = PenLineCap.Round }, Geometry);
                    }
                    else if (EndArrowType == 3)
                    {
                        Point pt = EndPoint.Value;
                        pt.X -= 3 * Thickness;
                        pt.Y -= 3 * Thickness;
                        dc.DrawGeometry(LineBrush, new Pen(LineBrush, Thickness), new RectangleGeometry(new Rect(pt, new Size(6 * Thickness, 6 * Thickness)), double.MaxValue, double.MaxValue));
                    }
                }
                if (StartArrow)
                {
                    if (StartArrowType == 1)
                    {
                        double theta = Math.Atan2(StartPoint.Value.Y - EndPoint.Value.Y, StartPoint.Value.X - EndPoint.Value.X);
                        double sint = Math.Sin(theta);
                        double cost = Math.Cos(theta);
                        var Geometry = new GeometryGroup()
                        {
                            Children ={new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                                    {
                                       new PathFigure()
                                            {
                                                StartPoint = new Point( StartPoint.Value.X - ((Thickness + 7) * cost + (Thickness + 3) * sint),
                                                                        StartPoint.Value.Y + ((Thickness + 3) * cost - (Thickness + 7) * sint)),
                                                Segments = {
                                                    new LineSegment() { Point = StartPoint.Value },
                                                    new LineSegment() { Point = new Point( StartPoint.Value.X - (((Thickness + 7) * cost) - ((Thickness + 3) * sint)),
                                                                                            StartPoint.Value.Y - (((Thickness + 7) * sint) + ((Thickness + 3) * cost))) } }
                                            }
                                    }
                        }
                    }
                        };
                        dc.DrawGeometry(Brushes.Transparent, new Pen(LineBrush, Thickness) { DashStyle = DashStyles.Solid, EndLineCap = PenLineCap.Round, StartLineCap = PenLineCap.Round }, Geometry);
                    }
                    else if (StartArrowType == 2)
                    {
                        double theta = Math.Atan2(StartPoint.Value.Y - EndPoint.Value.Y, StartPoint.Value.X - EndPoint.Value.X);
                        double sint = Math.Sin(theta);
                        double cost = Math.Cos(theta);
                        var Geometry = new GeometryGroup()
                        {
                            Children ={new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                                    {
                                       new PathFigure()
                                            {
                                                StartPoint = new Point( StartPoint.Value.X - ((Thickness + 7) * cost + (Thickness + 3) * sint),
                                                                        StartPoint.Value.Y + ((Thickness + 3) * cost - (Thickness + 7) * sint)),
                                                IsClosed=true,
                                                Segments = {
                                                    new LineSegment() { Point = StartPoint.Value },
                                                    new LineSegment() { Point = new Point( StartPoint.Value.X - (((Thickness + 7) * cost) - ((Thickness + 3) * sint)),
                                                                                            StartPoint.Value.Y - (((Thickness + 7) * sint) + ((Thickness + 3) * cost))) } }
                                            }
                                    }
                        }
                    }
                        };
                        dc.DrawGeometry(LineBrush, new Pen(LineBrush, Thickness) { DashStyle = DashStyles.Solid, EndLineCap = PenLineCap.Round, StartLineCap = PenLineCap.Round }, Geometry);
                    }
                    else if (StartArrowType == 3)
                    {
                        Point pt = StartPoint.Value;
                        pt.X -= 3 * Thickness;
                        pt.Y -= 3 * Thickness;
                        dc.DrawGeometry(LineBrush, new Pen(LineBrush, Thickness), new RectangleGeometry(new Rect(pt, new Size(6 * Thickness, 6 * Thickness)), double.MaxValue, double.MaxValue));
                    }
                }
            }
        }

        public double GetLeft()
        {
            return Math.Min(StartPoint.Value.X, EndPoint.Value.X);
        }

        public double GetTop()
        {
            return Math.Min(StartPoint.Value.Y, EndPoint.Value.Y);
        }

        public double GetWidth()
        {
            return (Math.Max(StartPoint.Value.X, EndPoint.Value.X) - Math.Min(StartPoint.Value.X, EndPoint.Value.X));
        }

        public double GetHeight()
        {
            return (Math.Max(StartPoint.Value.Y, EndPoint.Value.Y) - Math.Min(StartPoint.Value.Y, EndPoint.Value.Y));
        }

        public void SetLeft(double left)
        {
            StartPoint = new Point(StartPoint.Value.X - GetLeft() - left, StartPoint.Value.Y);
            EndPoint = new Point(EndPoint.Value.X - GetLeft() - left, EndPoint.Value.Y);
        }

        public void SetTop(double top)
        {
            StartPoint = new Point(StartPoint.Value.X, StartPoint.Value.Y - GetTop() - top);
            EndPoint = new Point(EndPoint.Value.X, EndPoint.Value.Y - GetTop() - top);
        }

        public void SetWidth(double width)
        {
            double Width = GetWidth();
            Matrix m = new Matrix();
            Point[] myArray = { StartPoint.Value, EndPoint.Value };
            m.ScaleAtPrepend(width / Width, 1, GetLeft(), GetTop());
            m.Transform(myArray);
            StartPoint = myArray[0];
            EndPoint = myArray[1];
        }

        public void SetHeight(double height)
        {
            Matrix m = new Matrix();
            Point[] myArray = { StartPoint.Value, EndPoint.Value };
            m.ScaleAtPrepend(1, height / GetHeight(), GetLeft(), GetTop());
            m.Transform(myArray);
            StartPoint = myArray[0];
            EndPoint = myArray[1];
        }

        public void Rotate(double angle)
        {
            double centerx = GetLeft() + GetWidth() / 2;
            double centery = GetTop() + GetHeight() / 2;
            Matrix m = new Matrix();
            Point[] myArray = { StartPoint.Value, EndPoint.Value };
            m.RotateAt(angle, centerx, centery);
            m.Transform(myArray);
            StartPoint = myArray[0];
            EndPoint = myArray[1];
        }

        public void RotateAroundPoint(double angle, Point center)
        {
            Matrix m = new Matrix();
            Point[] myArray = { StartPoint.Value, EndPoint.Value };
            m.RotateAt(angle, center.X, center.Y);
            m.Transform(myArray);
            StartPoint = myArray[0];
            EndPoint = myArray[1];
        }
    }
}
