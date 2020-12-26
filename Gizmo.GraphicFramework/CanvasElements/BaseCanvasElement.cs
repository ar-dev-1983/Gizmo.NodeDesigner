using Gizmo.GraphicFramework.Interfaces;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gizmo.GraphicFramework.CanvasElements
{
    public abstract class BaseCanvasElement : Shape, IShape
    {
        static BaseCanvasElement()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseCanvasElement), new FrameworkPropertyMetadata(typeof(BaseCanvasElement)));
        }

        public Guid Id
        {
            get => (Guid)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }
        public Guid ParentId
        {
            get => (Guid)GetValue(ParentIdProperty);
            set => SetValue(ParentIdProperty, value);
        }
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        public bool IsGroup
        {
            get => (bool)GetValue(IsGroupProperty);
            set => SetValue(IsGroupProperty, value);
        }
        public bool IsLocked
        {
            get => (bool)GetValue(IsLockedProperty);
            set => SetValue(IsLockedProperty, value);
        }
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }
        public PointCollection GeometryData
        {
            get => (PointCollection)GetValue(GeometryDataProperty);
            set => SetValue(GeometryDataProperty, value);
        }
        public Point? StartPoint
        {
            get => (Point?)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        public Point? EndPoint
        {
            get => (Point?)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }
        public Point? CenterPoint
        {
            get => (Point?)GetValue(CenterPointProperty);
            set => SetValue(CenterPointProperty, value);
        }
        public Brush LineBrush
        {
            get => (Brush)GetValue(LineBrushProperty);
            set => SetValue(LineBrushProperty, value);
        }
        public Brush FillBrush
        {
            get => (Brush)GetValue(FillBrushProperty);
            set => SetValue(FillBrushProperty, value);
        }
        public DashStyle LineDashStyle
        {
            get => (DashStyle)GetValue(LineDashStyleProperty);
            set => SetValue(LineDashStyleProperty, value);
        }
        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }
        public double XRadius
        {
            get => (double)GetValue(XRadiusProperty);
            set => SetValue(XRadiusProperty, value);
        }
        public double YRadius
        {
            get => (double)GetValue(YRadiusProperty);
            set => SetValue(YRadiusProperty, value);
        }
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(Guid), typeof(BaseCanvasElement), new UIPropertyMetadata(Guid.NewGuid()));
        public static readonly DependencyProperty ParentIdProperty = DependencyProperty.Register("ParentId", typeof(Guid), typeof(BaseCanvasElement), new UIPropertyMetadata(Guid.NewGuid()));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(BaseCanvasElement), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsGroupProperty = DependencyProperty.Register("ISgroup", typeof(bool), typeof(BaseCanvasElement), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsLockedProperty = DependencyProperty.Register("IsLocked", typeof(bool), typeof(BaseCanvasElement), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(BaseCanvasElement), new UIPropertyMetadata(false));
        public static readonly DependencyProperty GeometryDataProperty = DependencyProperty.Register("GeometryData", typeof(PointCollection), typeof(BaseCanvasElement), new UIPropertyMetadata(new PointCollection()));
        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register("StartPoint", typeof(Point?), typeof(BaseCanvasElement), new UIPropertyMetadata(null));
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register("EndPoint", typeof(Point?), typeof(BaseCanvasElement), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CenterPointProperty = DependencyProperty.Register("CenterPoint", typeof(Point?), typeof(BaseCanvasElement), new UIPropertyMetadata(null));
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush", typeof(Brush), typeof(BaseCanvasElement), new UIPropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty FillBrushProperty = DependencyProperty.Register("FillBrush", typeof(Brush), typeof(BaseCanvasElement), new UIPropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty LineDashStyleProperty = DependencyProperty.Register("LineDashStyle", typeof(DashStyle), typeof(BaseCanvasElement), new UIPropertyMetadata(DashStyles.Solid));
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(double), typeof(BaseCanvasElement), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty XRadiusProperty = DependencyProperty.Register("XRadius", typeof(double), typeof(BaseCanvasElement), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty YRadiusProperty = DependencyProperty.Register("YRadius", typeof(double), typeof(BaseCanvasElement), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(BaseCanvasElement), new UIPropertyMetadata(12d));
        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register("Rotation", typeof(double), typeof(BaseCanvasElement), new UIPropertyMetadata(0d));
    }

}
