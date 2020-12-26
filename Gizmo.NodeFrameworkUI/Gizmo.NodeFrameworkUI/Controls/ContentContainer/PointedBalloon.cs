using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Gizmo.NodeFrameworkUI
{
    public class PointedBalloon : Shape
    {
        #region Constructors
        static PointedBalloon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PointedBalloon), new FrameworkPropertyMetadata(typeof(PointedBalloon)));
        }
        public VHOrientation Orientation
        {
            get => (VHOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion

        #region Override Methods
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness), DefiningGeometry);
        }
        #endregion

        #region Override Properties
        protected override Geometry DefiningGeometry
        {
            get
            {
                Geometry geometry = null;
                if (Orientation == VHOrientation.Top)
                {
                    geometry = Geometry.Parse("m3 0c-1.7 0-3 1.4-3 3v" + ContentHeight.ToString() + "c0 1.7 1.4 3 3 3h" + Math.Round((ContentWidth-12)/2,0).ToString() + "l3 3 3-3h" + Math.Round((ContentWidth - 12) / 2, 0).ToString() + "c1.7 0 3-1.4 3-3v-" + ContentHeight.ToString() + "c0-1.7-1.4-3-3-3z");
                }
                else if (Orientation == VHOrientation.Bottom)
                {
                    geometry = Geometry.Parse("m3 " + ContentWidth.ToString() + "c-1.7 0-3-1.4-3-3v-" + ContentHeight.ToString() + "c0-1.7 1.4-3 3-3h" + Math.Round((ContentWidth - 12) / 2, 0).ToString() + "l3-3 3 3h" + Math.Round((ContentWidth - 12) / 2, 0).ToString() + "c1.7 0 3 1.4 3 3v" + ContentHeight.ToString() + "c0 1.7-1.4 3-3 3z");
                }
                else if (Orientation == VHOrientation.Left)
                {
                    geometry = Geometry.Parse("m0 3c0-1.7 1.4-3 3-3h" + ContentWidth.ToString() + "c1.7 0 3 1.4 3 3v" + Math.Round((ContentHeight - 12) / 2, 0).ToString() + "l3 3-3 3v" + Math.Round((ContentHeight - 12) / 2, 0).ToString() + "c0 1.7-1.4 3-3 3h-" + ContentWidth.ToString() + "c-1.7 0-3-1.4-3-3z");
                }
                else
                {
                    geometry = Geometry.Parse("m" + ContentHeight.ToString() + " 3c0-1.7-1.4-3-3-3h-" + ContentWidth.ToString() + "c-1.7 0-3 1.4-3 3v" + Math.Round((ContentHeight - 12) / 2, 0).ToString() + "l-3 3 3 3v" + Math.Round((ContentHeight - 12) / 2, 0).ToString() + "c0 1.7 1.4 3 3 3h" + ContentWidth.ToString() + "c1.7 0 3-1.4 3-3z");
                }
                return geometry;
            }
        }
        #endregion

        #region Public Methods
        private void UpdateRender()
        {
            InvalidateArrange();
            InvalidateMeasure();
            InvalidateVisual();
        }
        #endregion

        #region Private Methods
        private void ProcessHeader()
        {
            Effect = new DropShadowEffect()
            {

                Color = ShadowColor,
                BlurRadius = 5,
                Opacity = 0.5,
                ShadowDepth = 2,
                Direction = Orientation == VHOrientation.Left ? 180 : Orientation == VHOrientation.Right ? 0 : Orientation == VHOrientation.Top ? 90 : Orientation == VHOrientation.Bottom ? 270 : 0
            };
        }
        #endregion

        #region Public Properties
        public double ContentWidth
        {
            get => (double)GetValue(ContentWidthProperty);
            set => SetValue(ContentWidthProperty, value);
        }
        public double ContentHeight
        {
            get => (double)GetValue(ContentHeightProperty);
            set => SetValue(ContentHeightProperty, value);
        }
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(VHOrientation), typeof(PointedBalloon), new FrameworkPropertyMetadata(VHOrientation.Left, new PropertyChangedCallback(OrientationChanged)));
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(PointedBalloon), new FrameworkPropertyMetadata(0.0d, new PropertyChangedCallback(ContentSizeChanged)));
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(PointedBalloon), new FrameworkPropertyMetadata(0.0d, new PropertyChangedCallback(ContentSizeChanged)));
        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register("ShadowColor", typeof(Color), typeof(PointedBalloon), new FrameworkPropertyMetadata(Colors.Transparent, new PropertyChangedCallback(ShadowColorChanged)));
        #endregion

        #region Property Callbacks
        private static void OrientationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PointedBalloon header = (PointedBalloon)o;
            header.ProcessHeader();
            header.UpdateRender();
        }
        private static void ContentSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PointedBalloon header = (PointedBalloon)o;
            header.UpdateRender();
        }
        private static void ShadowColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PointedBalloon header = (PointedBalloon)o;
            header.ProcessHeader();
            header.UpdateRender();
        }
        #endregion
    }
}
