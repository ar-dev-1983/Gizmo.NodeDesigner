using Gizmo.NodeFramework;
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
    public enum ToolBoxPlacement
    {
        Top,
        Bottom
    }

    public class EngineToolBox : ContentControl
    {
        #region Private Properties
        private EngineToolBoxItem ZoomIn;
        private EngineToolBoxItem ZoomOut;
        private EngineToolBoxItem ShowZoomBox;
        private EngineZoomBox ZoomBox;

        private EngineToolBoxItem Play;
        private EngineToolBoxItem Pause;
        private EngineToolBoxItem Step;
        private EngineToolBoxItem Stop;

        private EngineToolBoxItem Settings;

        private EngineWrapper parentEngine;
        private EngineWrapper ParentEngine
        {
            get
            {
                if (parentEngine == null)
                {
                    parentEngine = VisualHelper.FindVisulaParent<EngineWrapper>(this);
                }
                return parentEngine;
            }
        }
        #endregion

        #region Constructors
        public EngineToolBox() : base()
        {
            DefaultStyleKey = typeof(EngineToolBox);
        }

        static EngineToolBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EngineToolBox), new FrameworkPropertyMetadata(typeof(EngineToolBox)));
        }
        #endregion

        #region Override Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ZoomIn = GetTemplateChild("PART_ZoomIn") as EngineToolBoxItem;
            ZoomOut = GetTemplateChild("PART_ZoomOut") as EngineToolBoxItem;
            ShowZoomBox = GetTemplateChild("PART_ShowZoomBox") as EngineToolBoxItem;

            Play = GetTemplateChild("PART_Play") as EngineToolBoxItem;
            Pause = GetTemplateChild("PART_Pause") as EngineToolBoxItem;
            Step = GetTemplateChild("PART_Step") as EngineToolBoxItem;
            Stop = GetTemplateChild("PART_Stop") as EngineToolBoxItem;

            Settings = GetTemplateChild("PART_Settings") as EngineToolBoxItem;

            ZoomBox = GetTemplateChild("PART_ZoomBox") as EngineZoomBox;

            Play.Click += Play_Click;
            Pause.Click += Pause_Click;
            Step.Click += Step_Click;
            Stop.Click += Stop_Click;

            ZoomIn.Click += ZoomIn_Click;
            ZoomOut.Click += ZoomOut_Click;
            ShowZoomBox.Click += ShowZoomBox_Click;

            Settings.Click += Settings_Click;

            if (ParentEngine != null)
            {
                ParentEngine.EngineChanged += ParentEngine_EngineChanged;
            }
        }

        private void ParentEngine_EngineChanged(object sender, RoutedEventArgs e)
        {
            if (ParentEngine != null)
            {
                if (ParentEngine.Source != null)
                {
                    if (ParentEngine.Source.State == EngineState.Play)
                    {
                        Play.Selected = true;
                    }
                    else if (ParentEngine.Source.State == EngineState.Stop)
                    {
                        Stop.Selected = true;
                    }
                    else if (ParentEngine.Source.State == EngineState.Pause)
                    {
                        Pause.Selected = true;
                    }
                }
            }
        }

        #region Engine Control
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Play.Selected = true;
                Pause.Selected = false;
                Stop.Selected = false;

                if (ParentEngine != null)
                {
                    ParentEngine.Source.Start();
                }
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Play.Selected = false;
                Pause.Selected = true;
                Stop.Selected = false;
                if (ParentEngine != null)
                {
                    ParentEngine.Source.Pause();
                }
            }
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Play.Selected = false;
                Pause.Selected = false;
                Stop.Selected = false;
                if (ParentEngine != null)
                {
                    ParentEngine.Source.Step();
                }
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Play.Selected = false;
                Pause.Selected = false;
                Stop.Selected = true;
                if (ParentEngine != null)
                {
                    ParentEngine.Source.Stop();
                }
            }
        }
        #endregion

        #region Zoom Control
        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && ZoomBox != null)
            {
                ZoomBox.ZoomIn();
            }
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && ZoomBox != null)
            {
                ZoomBox.ZoomOut();
            }
        }

        private void ShowZoomBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Settings.Selected = false;
                ShowZoomBox.Selected = !ShowZoomBox.Selected;
            }
        }
        #endregion

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                ShowZoomBox.Selected = false;
                Settings.Selected = !Settings.Selected;
            }
        }

        #endregion

        #region Public Methods
        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }
        public bool IsSettingsExpanded
        {
            get { return (bool)GetValue(IsSettingsExpandedProperty); }
            set { SetValue(IsSettingsExpandedProperty, value); }
        }
        public bool IsZoomBoxExpanded
        {
            get { return (bool)GetValue(IsZoomBoxExpandedProperty); }
            set { SetValue(IsZoomBoxExpandedProperty, value); }
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public ToolBoxPlacement Placement
        {
            get => (ToolBoxPlacement)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }
        public bool ShowGrid
        {
            get { return (bool)GetValue(ShowGridProperty); }
            set { SetValue(ShowGridProperty, value); }
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(EngineToolBox), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register("ScrollViewer", typeof(ScrollViewer), typeof(EngineToolBox));
        public static readonly DependencyProperty IsZoomBoxExpandedProperty = DependencyProperty.Register("IsZoomBoxExpanded", typeof(bool), typeof(EngineToolBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsSettingsExpandedProperty = DependencyProperty.Register("IsSettingsExpanded", typeof(bool), typeof(EngineToolBox), new UIPropertyMetadata(false));
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(ToolBoxPlacement), typeof(EngineToolBox), new UIPropertyMetadata(ToolBoxPlacement.Top));
        public static readonly DependencyProperty ShowGridProperty = DependencyProperty.Register("ShowGrid", typeof(bool), typeof(EngineToolBox), new UIPropertyMetadata(false));
        #endregion
    }
}
