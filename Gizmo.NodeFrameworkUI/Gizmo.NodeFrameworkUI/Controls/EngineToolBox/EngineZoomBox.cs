using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{
    public class EngineZoomBox : Control
    {

        #region Private Properties
        private Thumb miniatureThumb;
        private Canvas zoomBoxCanvas;
        private ScaleTransform scaleTransform;
        private Canvas SourceCanvas;
        private readonly List<double> ZoomValues = new List<double> { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 150, 200, 300, 400, 500, 0 };
        private int ZoomIndex = 10;
        private double ZoomFactor = 0;
        #endregion

        #region Constructors
        public EngineZoomBox() : base()
        {
            DefaultStyleKey = typeof(EngineZoomBox);
        }

        static EngineZoomBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EngineZoomBox), new FrameworkPropertyMetadata(typeof(EngineZoomBox)));
        }
        #endregion

        #region Override Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateSource();
        }
        #endregion

        #region Private Methods
        private void UpdateSource()
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                if (Source != null)
                {
                    SourceCanvas = VisualHelper.FindChild<NodeCanvas>(Source);
                    if (SourceCanvas != null)
                    {
                        miniatureThumb = GetTemplateChild("PART_MiniatureThumb") as Thumb;
                        zoomBoxCanvas = GetTemplateChild("PART_ZoomBoxCanvas") as Canvas;
                        if (miniatureThumb != null && zoomBoxCanvas != null)
                        {
                            SourceCanvas.LayoutUpdated += SourceCanvas_LayoutUpdated;
                            miniatureThumb.DragDelta += Thumb_DragDelta;
                            scaleTransform = new ScaleTransform();
                            SourceCanvas.LayoutTransform = scaleTransform;
                            ZoomFactor = ZoomValues[ZoomIndex];
                            ZoomValue = ZoomFactor.ToString() + " %";
                        }
                    }
                }
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            InvalidateScale(out double scale, out double xOffset, out double yOffset);
            Source.ScrollToHorizontalOffset(Source.HorizontalOffset + e.HorizontalChange / scale);
            Source.ScrollToVerticalOffset(Source.VerticalOffset + e.VerticalChange / scale);
        }

        private void SourceCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                InvalidateScale(out double scale, out double xOffset, out double yOffset);
                miniatureThumb.Width = Source.ViewportWidth * scale;
                miniatureThumb.Height = Source.ViewportHeight * scale;
                Canvas.SetLeft(miniatureThumb, xOffset + Source.HorizontalOffset * scale);
                Canvas.SetTop(miniatureThumb, yOffset + Source.VerticalOffset * scale);
            }
        }

        private void InvalidateScale(out double scale, out double xOffset, out double yOffset)
        {
            double Width = SourceCanvas.ActualWidth * scaleTransform.ScaleX;
            double Height = SourceCanvas.ActualHeight * scaleTransform.ScaleY;
            double X = zoomBoxCanvas.ActualWidth;
            double Y = zoomBoxCanvas.ActualHeight;
            double scaleX = X / Width;
            double scaleY = Y / Height;
            scale = (scaleX < scaleY) ? scaleX : scaleY;
            xOffset = (X - scale * Width) / 2;
            yOffset = (Y - scale * Height) / 2;
        }

        private void Zoom()
        {
            var CurrentZoom = ZoomValues[ZoomIndex];
            double scale = CurrentZoom / ZoomFactor;
            SourceCanvas.UpdateLayout();
            scaleTransform.ScaleX *= scale;
            scaleTransform.ScaleY *= scale;
        }
        #endregion

        #region Public Methods
        public void ZoomIn()
        {
            if (ZoomIndex < ZoomValues.Count - 2)
            {
                ZoomIndex++;
                Zoom();
                ZoomFactor = ZoomValues[ZoomIndex];
                ZoomValue = ZoomFactor.ToString() + " %";
            }
        }
        public void ZoomOut()
        {
            if (ZoomIndex > 1)
            {
                ZoomIndex--;
                Zoom();
                ZoomFactor = ZoomValues[ZoomIndex];
                ZoomValue = ZoomFactor.ToString() + " %";
            }
        }
        #endregion

        #region Public Properties
        public ScrollViewer Source
        {
            get { return (ScrollViewer)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public string ZoomValue
        {
            get => (string)GetValue(ZoomValueProperty);
            set => SetValue(ZoomValueProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ScrollViewer), typeof(EngineZoomBox), new UIPropertyMetadata(null, OnSourceChanged));
        public static readonly DependencyProperty ZoomValueProperty = DependencyProperty.Register("ZoomValue", typeof(string), typeof(EngineZoomBox), new UIPropertyMetadata(string.Empty));
        #endregion

        #region Dependency Properties callbacks
        private static void OnSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                EngineZoomBox zoomBox = o as EngineZoomBox;
                zoomBox.UpdateSource();
            }
        }
        #endregion
    }
}
