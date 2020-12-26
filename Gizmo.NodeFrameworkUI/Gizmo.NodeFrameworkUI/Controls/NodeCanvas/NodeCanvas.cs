using Gizmo.NodeBase;
using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gizmo.NodeFrameworkUI
{
    public partial class NodeCanvas : Canvas
    {
        static NodeCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeCanvas), new FrameworkPropertyMetadata(typeof(NodeCanvas)));
        }
        public NodeCanvas() : base()
        {
            DefaultStyleKey = typeof(NodeCanvas);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AllowDrop = true;
            Loaded -= NodeCanvas_Loaded;
            Loaded += NodeCanvas_Loaded;

        }

        private void NodeCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            OnShowGrid();
        }

        #region Private Properties
        private Point? rubberbandSelectionStartPoint = null;
        private Point? rubberbandSelectionEndPoint = null;
        private protected EngineWrapper parentEngine;
        internal EngineWrapper ParentEngine
        {
            get
            {
                if (parentEngine is null)
                {
                    parentEngine = VisualHelper.FindParent<EngineWrapper>(this);
                }
                return parentEngine;
            }
        }
        #endregion

        #region Public Properties
        public int GridDelta
        {
            get => (int)GetValue(GridDeltaProperty);
            set => SetValue(GridDeltaProperty, value);
        }
        public bool GridOn
        {
            get => (bool)GetValue(GridOnProperty);
            set => SetValue(GridOnProperty, value);
        }
        public bool ShowGrid
        {
            get => (bool)GetValue(ShowGridProperty);
            set => SetValue(ShowGridProperty, value);
        }
        public bool DesignMode
        {
            get => (bool)GetValue(DesignModeProperty);
            set => SetValue(DesignModeProperty, value);
        }
        public SolidColorBrush GridStroke
        {
            get => (SolidColorBrush)GetValue(GridStrokeProperty);
            set => SetValue(GridStrokeProperty, value);
        }
        public SolidColorBrush GridBackground
        {
            get => (SolidColorBrush)GetValue(GridBackgroundProperty);
            set => SetValue(GridBackgroundProperty, value);
        }
        public LinkStyle LinkStyle
        {
            get => (LinkStyle)GetValue(LinkStyleProperty);
            set => SetValue(LinkStyleProperty, value);
        }
        #endregion

        public static readonly DependencyProperty GridDeltaProperty = DependencyProperty.Register("GridDelta", typeof(int), typeof(NodeCanvas), new UIPropertyMetadata(10));
        public static readonly DependencyProperty GridOnProperty = DependencyProperty.Register("GridOn", typeof(bool), typeof(NodeCanvas), new UIPropertyMetadata(true));
        public static readonly DependencyProperty ShowGridProperty = DependencyProperty.Register("ShowGrid", typeof(bool), typeof(NodeCanvas), new UIPropertyMetadata(false, new PropertyChangedCallback(OnShowGridChanged)));
        public static readonly DependencyProperty DesignModeProperty = DependencyProperty.Register("DesignMode", typeof(bool), typeof(NodeCanvas), new UIPropertyMetadata(true, new PropertyChangedCallback(DesignModeChange)));
        public static readonly DependencyProperty GridStrokeProperty = DependencyProperty.Register("GridStroke", typeof(SolidColorBrush), typeof(NodeCanvas), new UIPropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty GridBackgroundProperty = DependencyProperty.Register("GridBackground", typeof(SolidColorBrush), typeof(NodeCanvas), new UIPropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty LinkStyleProperty = DependencyProperty.Register("LinkStyle", typeof(LinkStyle), typeof(NodeCanvas), new UIPropertyMetadata(LinkStyle.RoutePath, new PropertyChangedCallback(OnLinkStyleChange)));
        
        private static void OnLinkStyleChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            foreach (var node in ((NodeCanvas)d).Children)
            {
                if (node is LinkWrapper)
                {
                    (node as LinkWrapper).LinkStyle = ((NodeCanvas)d).LinkStyle;
                }
            }
        }
        
        private static void DesignModeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnShowGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NodeCanvas)d).OnShowGrid();
        }

        public void OnShowGrid()
        {
            if (this != null)
            {
                if (ShowGrid)
                {
                    Background = new VisualBrush()
                    {
                        TileMode = TileMode.Tile,
                        Stretch = Stretch.None,
                        ViewportUnits = BrushMappingMode.Absolute,
                        ViewboxUnits = BrushMappingMode.Absolute,
                        Viewport = new Rect(0, 0, 10 * GridDelta, 10 * GridDelta),
                        Viewbox = new Rect(0, 0, 10 * GridDelta, 10 * GridDelta),
                        Visual = new Grid()
                        {
                            Background = GridBackground,
                            Children =
                            {
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 0 L " + (10*GridDelta).ToString() + " 0 L " + (10*GridDelta).ToString() + " " + (10*GridDelta).ToString()+ " L 0 " + (10*GridDelta).ToString()+ " L 0 0"),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 1,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + GridDelta.ToString() + " 0 L " + GridDelta.ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + (2*GridDelta).ToString() + " 0 L " + (2*GridDelta).ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + (3*GridDelta).ToString() + " 0 L " + (3*GridDelta).ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + (4*GridDelta).ToString() + " 0 L " + (4*GridDelta).ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + (5*GridDelta).ToString() + " 0 L " + (5*GridDelta).ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + (6*GridDelta).ToString() + " 0 L " + (6*GridDelta).ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + (7*GridDelta).ToString() + " 0 L " + (7*GridDelta).ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + (8*GridDelta).ToString() + " 0 L " + (8*GridDelta).ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M " + (9*GridDelta).ToString() + " 0 L " + (9*GridDelta).ToString() + " " + (10*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + GridDelta.ToString() + " L " + (10*GridDelta).ToString() + " " + GridDelta.ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + (2*GridDelta).ToString() + " L " + (10*GridDelta).ToString() + " " + (2*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + (3*GridDelta).ToString() + " L " + (10*GridDelta).ToString() + " " + (3*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + (4*GridDelta).ToString() + " L " + (10*GridDelta).ToString() + " " + (4*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + (5*GridDelta).ToString() + " L " + (10*GridDelta).ToString() + " " + (5*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + (6*GridDelta).ToString() + " L " + (10*GridDelta).ToString() + " " + (6*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + (7*GridDelta).ToString() + " L " + (10*GridDelta).ToString() + " " + (7*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + (8*GridDelta).ToString() + " L " + (10*GridDelta).ToString() + " " + (8*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                },
                                new Path()
                                {
                                Data = Geometry.Parse("M 0 " + (9*GridDelta).ToString() + " L " + (10*GridDelta).ToString() + " " + (9*GridDelta).ToString()),
                                Fill = Brushes.Transparent,
                                StrokeThickness = 0.4,
                                Stroke = GridStroke
                                }
                            }
                        }
                    };
                }
                else
                {
                    Background = GridBackground;
                }
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();

            foreach (UIElement element in InternalChildren)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            size.Width += 10;
            size.Height += 10;
            return size;

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (DesignMode)
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    rubberbandSelectionStartPoint = null;
                    rubberbandSelectionEndPoint = null;
                }
                else
                {

                    if (GridOn)
                        rubberbandSelectionEndPoint = new Point?(NodeCanvasHelper.AdjustPointToGrid(e.GetPosition(this), GridOn, GridDelta));
                    else
                        rubberbandSelectionEndPoint = new Point?(e.GetPosition(this));

                    if (rubberbandSelectionStartPoint.HasValue && rubberbandSelectionEndPoint.HasValue)
                    {
                        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                        if (adornerLayer != null)
                        {
                            NodeDesignerSelectionAdorner adorner = new NodeDesignerSelectionAdorner(this, rubberbandSelectionStartPoint);
                            if (adorner != null)
                            {
                                adornerLayer.Add(adorner);
                            }
                        }
                    }
                }
                e.Handled = true;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseMove(e);
            if (DesignMode)
            {
                if (e.Source is Canvas)
                {
                    if (GridOn)
                        rubberbandSelectionStartPoint = new Point?(NodeCanvasHelper.AdjustPointToGrid(e.GetPosition(this), GridOn, GridDelta));
                    else
                        rubberbandSelectionStartPoint = new Point?(e.GetPosition(this));
                    ClearSelection();
                    Focus();
                }
                e.Handled = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (DesignMode)
            {
                if (e.Key == Key.Delete)
                {
                    ParentEngine.RemoveSelected();
                }
                else if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
                {
                }
                else if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
                {
                }
                else if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
                {
                }
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (DesignMode)
            {
                if (e.Data.GetData(typeof(DragObject)) is DragObject dragObject)
                {
                    if (dragObject.node != null && dragObject.node is Node)
                    {
                        AddDropNode(dragObject.node, NodeCanvasHelper.AdjustPointToGrid(e.GetPosition(this), GridOn, GridDelta));
                    }
                }
                e.Handled = true;
            }
        }

        public void AddDropNode(Node node, Point Position)
        {
            if (node != null)
            {
                if (DesignMode)
                {
                    node.Position = new EntityPosition(Position.X, Position.Y);
                    node.ModuleId = ParentEngine.Source.SelectedModule.Id;
                    ParentEngine.Source.AddNode(node);
                }
            }
        }

        public void AddLink(Link link)
        {
            if (ParentEngine != null && link != null)
            {
                if (DesignMode)
                {
                    VariableWrapper inputWrapper = null;
                    VariableWrapper outputWrapper = null;
                    var InputNodeId = ParentEngine.Source.GetNodeByInputId(link.InputId).Id;
                    var OutputNodeId = ParentEngine.Source.GetNodeByOutputId(link.OutputId).Id;

                    foreach (var node in Children)
                    {
                        if ((node is NodeWrapper))
                        {
                            if ((node as NodeWrapper).Id == InputNodeId)
                            {
                                inputWrapper = (node as NodeWrapper).InputConnectors.Where(x => (x as VariableWrapper).Variable.Id == link.InputId).First() as VariableWrapper;
                            }
                            if ((node as NodeWrapper).Id == OutputNodeId)
                            {
                                outputWrapper = (node as NodeWrapper).OutputConnectors.Where(x => (x as VariableWrapper).Variable.Id == link.OutputId).First() as VariableWrapper;
                            }
                        }
                    }
                    if (outputWrapper != null && inputWrapper != null)
                    {
                        ParentEngine.Source.AddLink(outputWrapper.Variable.Id, inputWrapper.Variable.Id);
                    }
                }
            }
        }

        public void AddLink(VariableWrapper Destination, VariableWrapper Source)
        {
            if (ParentEngine != null && Destination != null && Source != null)
            {
                if (DesignMode)
                {
                    if (Destination != null && Source != null)
                    {
                        ParentEngine.Source.AddLink(Destination.Variable.Id, Source.Variable.Id);
                    }
                }
            }
        }

        public void UpdateLink(LinkWrapper link, VariableWrapper oldVariable, VariableWrapper newVariable)
        {
            if (ParentEngine != null && link != null && oldVariable != null && newVariable != null)
            {
                if (DesignMode)
                {
                    var ChangedLink = ParentEngine.Source.GetLink(link.Link);
                    if (ChangedLink != null)
                    {
                        if (oldVariable.Variable.VariableType == VariableType.Input)
                        {
                            ChangedLink.InputId = newVariable.Variable.Id;
                        }
                        else if (oldVariable.Variable.VariableType == VariableType.Output)
                        {
                            ChangedLink.OutputId = newVariable.Variable.Id;
                        }
                    }
                }
            }
        }

        public void SelectItem(EntityWrapper item)
        {
            ParentEngine.SelectionService.SelectItem(item);
        }

        public void AddToSelection(EntityWrapper item)
        {
            ParentEngine.SelectionService.AddToSelection(item);
        }

        public void RemoveFromSelection(EntityWrapper item)
        {
            ParentEngine.SelectionService.RemoveFromSelection(item);
        }

        public void ClearSelection()
        {
            ParentEngine.SelectionService.ClearSelection();
        }

        public void SelectAll()
        {
            ParentEngine.SelectionService.SelectAll();
        }

        public List<EntityWrapper> GetGroupMembers(EntityWrapper item)
        {
            return ParentEngine.SelectionService.GetGroupMembers(item);
        }

        public EntityWrapper GetGroupRoot(EntityWrapper item)
        {
            return ParentEngine.SelectionService.GetGroupRoot(item);
        }
        
        public List<EntityWrapper> CurrentSelection()
        {
            return ParentEngine.SelectionService.CurrentSelection;
        }

        public SelectionService SelectionService()
        {
            return ParentEngine.SelectionService;
        }
    }
}
