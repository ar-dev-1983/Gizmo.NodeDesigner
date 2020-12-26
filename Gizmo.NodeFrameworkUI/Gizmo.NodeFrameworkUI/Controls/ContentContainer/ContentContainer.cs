using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{
    public class ContentContainer : Decorator
    {
        static ContentContainer()
        {
        }

        public ContentContainer() : base()
        {
        }
        protected Geometry DefiningGeometry
        {
            get
            {
                Geometry geometry = null;
                if (Orientation == VHOrientation.Top)
                {
                    geometry = Geometry.Parse("m5 0c-1.7 0-5 1.4-5 5v" + (ActualHeight-10).ToString() + "c0 1.7 1.4 5 5 5h" + Math.Round((ActualWidth - 20) / 2, 0) + "l5 5 5-5h" + Math.Round((ActualWidth - 20) / 2, 0) + "c1.7 0 5-1.4 5-5v-" + (ActualHeight-10).ToString() + "c0-1.7-1.4-5-5-5z");
                }
                else if (Orientation == VHOrientation.Bottom)
                {
                    geometry = Geometry.Parse("m5 " + ActualHeight.ToString() + "c-1.7 0-5-1.4-5-5v-" + (ActualHeight-10).ToString() + "c0-1.7 1.4-5 5-5h" + Math.Round((ActualWidth - 20) / 2, 0) + "l5-5 5 5h" + Math.Round((ActualWidth - 20) / 2, 0) + "c1.7 0 5 1.4 5 5v" + (ActualHeight-10).ToString() + "c0 1.7-1.4 5-5 5z");
                }
                else if (Orientation == VHOrientation.Left)
                {
                    geometry = Geometry.Parse("m0 5c0-1.7 1.4-5 5-5h" + (ActualWidth-10).ToString() + "c1.7 0 5 1.4 5 5v" + Math.Round((ActualHeight - 20) / 2, 0) + "l5 5-5 5v" + Math.Round((ActualHeight - 20) / 2, 0) + "c0 1.7-1.4 5-5 5h-" + (ActualWidth-10).ToString() + "c-1.7 0-5-1.4-5-5z");
                }
                else
                {
                    geometry = Geometry.Parse("m" + ActualWidth.ToString() + " 5c0-1.7-1.4-5-5-5h-" + (ActualWidth-10).ToString() + "c-1.7 0-5 1.4-5 5v" + Math.Round((ActualHeight - 20) / 2, 0) + "l-5 5 5 5v" + Math.Round((ActualHeight - 20) / 2, 0) + "c0 1.7 1.4 5 5 5h" + (ActualWidth-10).ToString() + "c1.7 0 5-1.4 5-5z");
                }
                return geometry;
            }
        }
        private static Size HelperCollapseThickness(Thickness th)
        {
            return new Size(th.Left + th.Right, th.Top + th.Bottom);
        }
        private static double GetThickness(Thickness th)
        {
            return Math.Max(0.0, Math.Max(th.Bottom, Math.Max(th.Top, Math.Max(th.Right, th.Left))));
        }
        private static Rect HelperDeflateRect(Rect rt, Thickness thick)
        {
            return new Rect(rt.Left + thick.Left, rt.Top + thick.Top, Math.Max(0.0, rt.Width - thick.Left - thick.Right), Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
        }
        protected override Size MeasureOverride(Size constraint)
        {
            UIElement child = Child;
            Size mySize = new Size();
            Thickness borders = BorderThickness;
            Size border = HelperCollapseThickness(borders);
            if (child != null)
            {
                Size childConstraint = new Size(Math.Max(0.0, constraint.Width - border.Width), Math.Max(0.0, constraint.Height - border.Height));
                child.Measure(childConstraint);
                Size childSize = child.DesiredSize;
                mySize.Width = childSize.Width + border.Width;
                mySize.Height = childSize.Height + border.Height;
            }
            else
            {
                mySize = new Size(border.Width, border.Height);
            }

            return mySize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Thickness borders = BorderThickness;
            Rect boundRect = new Rect(finalSize);
            Rect innerRect = HelperDeflateRect(boundRect, borders);
            UIElement child = Child;
            if (child != null)
            {
                Rect childRect = HelperDeflateRect(innerRect, new Thickness(1));
                child.Arrange(childRect);
            }
            return finalSize;
        }
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(Background, new Pen(BorderBrush, GetThickness(BorderThickness)), DefiningGeometry);
        }

        public VHOrientation Orientation
        {
            get { return (VHOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(VHOrientation), typeof(ContentContainer), new FrameworkPropertyMetadata(VHOrientation.Left));
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(ContentContainer), new UIPropertyMetadata(new Thickness(0)));
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(ContentContainer), new FrameworkPropertyMetadata((Brush)null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(ContentContainer), new FrameworkPropertyMetadata((Brush)null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));
    }
}
