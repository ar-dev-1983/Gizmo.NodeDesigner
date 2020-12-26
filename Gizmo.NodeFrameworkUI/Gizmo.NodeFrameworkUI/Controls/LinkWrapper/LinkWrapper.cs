using Gizmo.NodeBase;
using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Gizmo.NodeFrameworkUI
{
    public partial class LinkWrapper : EntityWrapper, INotifyPropertyChanged
    {    
        public new EntityTypeEnum EntityType => EntityTypeEnum.Link;
        internal Link Link => DataContext != null ? DataContext is Link ? DataContext as Link : null : null;

        public LinkWrapper()
        {
            DefaultStyleKey = typeof(LinkWrapper);
            Unloaded += LinkWrapper_Unloaded;
            Loaded += LinkWrapper_Loaded;
        }

        private void LinkWrapper_Loaded(object sender, RoutedEventArgs e)
        {
            if (Link != null)
            {
                CreateLink();
                OnSelected += LinkWrapper_OnSelected;
                OnUnselected += LinkWrapper_OnUnselected;
            }
        }

        private void LinkWrapper_OnUnselected(object sender, RoutedEventArgs e)
        {
            HideAdorner();
        }

        private void LinkWrapper_OnSelected(object sender, RoutedEventArgs e)
        {
            ShowAdorner();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void CreateLink()
        {
            if (ParentCanvas != null)
            {
                var SourceNodeId = ParentCanvas.ParentEngine.Source.GetNodeByOutputId(Link.OutputId).Id;
                var DestinationId = ParentCanvas.ParentEngine.Source.GetNodeByInputId(Link.InputId).Id;

                foreach (var node in ParentCanvas.Children)
                {
                    if ((node is NodeWrapper) && (node as NodeWrapper).Id == SourceNodeId)
                    {
                        foreach (var variable in (node as NodeWrapper).OutputConnectors)
                        {
                            if ((variable is VariableWrapper) && (variable as VariableWrapper).Variable.Id == Link.OutputId)
                            {
                                Source = (variable as VariableWrapper);
                            }
                        }
                    }
                    else if ((node is NodeWrapper) && (node as NodeWrapper).Id == DestinationId)
                    {
                        foreach (var variable in (node as NodeWrapper).InputConnectors)
                        {
                            if ((variable is VariableWrapper) && (variable as VariableWrapper).Variable.Id == Link.InputId)
                            {
                                Destination = (variable as VariableWrapper);
                            }
                        }
                    }
                }
                if (Source != null && Destination != null)
                {
                    AnchorPositionSource = Source.Position;
                    AnchorPositionDestination = Destination.Position;
                }
            }
        }

        void LinkWrapper_Unloaded(object sender, RoutedEventArgs e)
        {

            if (linkWrapperAdorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(linkWrapperAdorner);
                    linkWrapperAdorner = null;
                }
            }
            OnSelected -= LinkWrapper_OnSelected;
            OnUnselected -= LinkWrapper_OnUnselected;

            Source = null;
            Destination = null;
        }
        private Adorner linkWrapperAdorner;

        private VariableWrapper source;
        public VariableWrapper Source
        {
            get => source;
            set
            {
                if (source != value)
                {
                    if (source != null)
                    {
                        source.Variable.IsConnected = false;
                        UpdateLink(this, source, value);
                        source.PropertyChanged -= new PropertyChangedEventHandler(OnConnectionPointPositionChanged);
                        source.Links.Remove(this);
                    }

                    source = value;

                    if (source != null)
                    {
                        source.Variable.IsConnected = true;
                        source.Links.Add(this);
                        source.PropertyChanged += new PropertyChangedEventHandler(OnConnectionPointPositionChanged);
                    }

                    UpdateLinkGeometry();
                }
            }
        }
        private VariableWrapper destination;
        public VariableWrapper Destination
        {
            get => destination;
            set
            {
                if (destination != value)
                {
                    if (destination != null)
                    {
                        destination.Variable.IsConnected = false;
                        UpdateLink(this, destination, value);

                        destination.PropertyChanged -= new PropertyChangedEventHandler(OnConnectionPointPositionChanged);
                        destination.Links.Remove(this);
                    }

                    destination = value;

                    if (destination != null)
                    {
                        destination.Variable.IsConnected = true;
                        destination.Links.Add(this);
                        destination.PropertyChanged += new PropertyChangedEventHandler(OnConnectionPointPositionChanged);
                    }
                    UpdateLinkGeometry();
                }
            }
        }
        private PathGeometry linkGeometry;
        public PathGeometry LinkGeometry
        {
            get { return linkGeometry; }
            set
            {
                if (linkGeometry != value)
                {
                    linkGeometry = value;
                    OnPropertyChanged("LinkGeometry");
                }
            }
        }

        private Point anchorPositionSource;
        public Point AnchorPositionSource
        {
            get { return anchorPositionSource; }
            set
            {
                if (anchorPositionSource != value)
                {
                    anchorPositionSource = value;
                    OnPropertyChanged("AnchorPositionSource");
                }
            }
        }
        private Point anchorPositionDestination;
        public Point AnchorPositionDestination
        {
            get { return anchorPositionDestination; }
            set
            {
                if (anchorPositionDestination != value)
                {
                    anchorPositionDestination = value;
                    OnPropertyChanged("AnchorPositionDestination");
                }
            }
        }

        private protected NodeCanvas parentCanvas;
        internal NodeCanvas ParentCanvas
        {
            get
            {
                if (parentCanvas is null)
                {
                    parentCanvas = VisualHelper.FindParent<NodeCanvas>(this);
                }
                return parentCanvas;
            }
        }
        public LinkStyle LinkStyle
        {
            get => (LinkStyle)GetValue(LinkStyleProperty);
            set => SetValue(LinkStyleProperty, value);
        }

        public static readonly DependencyProperty LinkStyleProperty = DependencyProperty.Register("LinkStyle", typeof(LinkStyle), typeof(LinkWrapper), new UIPropertyMetadata(LinkStyle.RoutePath, new PropertyChangedCallback(OnLinkStyleChange)));

        private static void OnLinkStyleChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LinkWrapper)d).UpdateLinkGeometry();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal void HideAdorner()
        {
            if (linkWrapperAdorner != null)
                linkWrapperAdorner.Visibility = Visibility.Collapsed;
        }

        private void ShowAdorner()
        {
            if (linkWrapperAdorner == null)
            {
                if (ParentCanvas!=null)
                {
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                    if (adornerLayer != null)
                    {
                        linkWrapperAdorner = new LinkWrapperAdorner(ParentCanvas, this);
                        adornerLayer.Add(linkWrapperAdorner);
                    }
                }
            }
            linkWrapperAdorner.Visibility = Visibility.Visible;
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (ParentCanvas!=null)
            {
                if (ParentCanvas.DesignMode)
                {
                    if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                        if (IsSelected)
                        {
                            ParentCanvas.RemoveFromSelection(this);
                        }
                        else
                        {
                            ParentCanvas.AddToSelection(this);
                        }
                    else if (!IsSelected)
                    {
                        ParentCanvas.SelectItem(this);
                    }
                    Focus();
                }
            }
            e.Handled = false;
        }

        void OnConnectionPointPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
            {
                UpdateLinkGeometry();
            }
        }
        private void UpdateLink(LinkWrapper link, VariableWrapper oldVariable, VariableWrapper newVariable)
        {
            if (ParentCanvas!=null)
            {
                if (ParentCanvas.DesignMode)
                {
                    ParentCanvas.UpdateLink(link, oldVariable, newVariable);
                }
            }
        }

        private void UpdateLinkGeometry()
        {
            if (Source != null && Destination != null)
            {
                PathGeometry geometry = new PathGeometry();

                List<Point> LinkPoints = GetLink(Source.GetInfo(), Destination.GetInfo(), LinkStyle);

                if (LinkStyle == LinkStyle.RoutePath)
                {
                    LinkPoints.Insert(0, new Point(Source.Position.X + 5, Source.Position.Y));
                    LinkPoints.Add(new Point(Destination.Position.X - 5, Destination.Position.Y));
                }
                if (LinkPoints.Count > 0)
                {
                    PathFigure figure = new PathFigure
                    {
                        StartPoint = LinkPoints[0]
                    };
                    LinkPoints.Remove(LinkPoints[0]);
                    figure.Segments.Add(LinkStyle == LinkStyle.RoutePath || LinkStyle == LinkStyle.Simple ? new PolyLineSegment(LinkPoints, true) as PathSegment : new PolyBezierSegment(LinkPoints, true) as PathSegment);
                    geometry.Figures.Add(figure);

                    LinkGeometry = geometry;
                    AnchorPositionDestination = Destination.GetInfo().Position;
                    AnchorPositionSource = Source.GetInfo().Position;
                }

            }
        }
    }
}
