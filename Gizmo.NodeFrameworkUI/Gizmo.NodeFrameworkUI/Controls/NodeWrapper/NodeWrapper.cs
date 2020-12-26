using Gizmo.NodeBase;
using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Gizmo.NodeFrameworkUI
{
    public class NodeWrapper : EntityWrapper
    {
        public new EntityTypeEnum EntityType => EntityTypeEnum.Node;

        private ItemsControl IC_InputConnectors;
        private ItemsControl IC_OutputConnectors;
        private ItemsControl IC_InternalPropertyItems;

        internal Node Node => DataContext != null ? DataContext is Node ? DataContext as Node : null : null;

        public NodeWrapper()
 : base()
        {
            InputConnectors = new ObservableCollection<UIElement>();
            OutputConnectors = new ObservableCollection<UIElement>();
            InternalPropertyItems = new ObservableCollection<UIElement>();

            DefaultStyleKey = typeof(NodeWrapper);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Node != null)
            {
                InputConnectors.Clear();
                OutputConnectors.Clear();
                InternalPropertyItems.Clear();

                FillInputConnectors();
                FillOutputConnectors();
                FillSettings();
                UpdateNodeWrapperSize();
            }
            SizeChanged += NodeWrapper_SizeChanged;
            Loaded += NodeWrapper_Loaded;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (VisualHelper.FindParent<NodeCanvas>(this) is NodeCanvas nodeDesigner)
            {
                if (nodeDesigner.DesignMode && !ToolboxMode)
                {
                    if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                        if (IsSelected)
                        {
                            nodeDesigner.RemoveFromSelection(this);
                        }
                        else
                        {
                            nodeDesigner.AddToSelection(this);
                        }
                    else if (!IsSelected)
                    {
                        nodeDesigner.SelectItem(this);
                    }
                    Focus();
                }
                e.Handled = false;
            }
        }
        
        private void UpdateNodeWrapperSize()
        {
            if (Node.Size.Width == 0 && Node.Size.Height == 0)
            {
                if (Node.UseInputs)
                {
                    IC_InputConnectors.Height = 0;
                    foreach (var item in InputConnectors)
                    {
                        (item as UIElement).Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        IC_InputConnectors.Height += (item as UIElement).DesiredSize.Height;
                    }
                    IC_InputConnectors.MinHeight = IC_InputConnectors.Height;
                }
                if (Node.UseOutputs)
                {
                    IC_OutputConnectors.Height = 0;
                    foreach (var item in OutputConnectors)
                    {
                        (item as UIElement).Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        IC_OutputConnectors.Height += (item as UIElement).DesiredSize.Height;
                    }
                    IC_OutputConnectors.MinHeight = IC_OutputConnectors.Height;
                }
                if (Node.UseSettings)
                {
                    IC_InternalPropertyItems.Height = 0;
                    foreach (var item in InternalPropertyItems)
                    {
                        (item as UIElement).Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        IC_InternalPropertyItems.Height += (item as UIElement).DesiredSize.Height;
                    }
                    IC_InternalPropertyItems.MinHeight = IC_InternalPropertyItems.Height;
                }
                if (GetTemplateChild("PART_MainDock") is DockPanel Dock)
                {
                    Dock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    var SizeRect = NodeCanvasHelper.AdjustRectToGrid(new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), Dock.DesiredSize.Width, Dock.DesiredSize.Height), true, 10);
                    MinHeight = SizeRect.Height;
                    MinWidth = SizeRect.Width;
                }
            }
        }
        
        private void FillInputConnectors()
        {
            if (Node.UseInputs)
            {
                IC_InputConnectors = GetTemplateChild("PART_InputConnectors") as ItemsControl;
                IC_InputConnectors.Visibility = Visibility.Visible;
                foreach (var item in Node.Inputs)
                {
                    if (item.VariableType == VariableType.Input)
                    {
                        var variable = new VariableWrapper(item);
                        var Binding = new Binding() { Path = new PropertyPath("ToolboxMode"), Source = this, Mode = BindingMode.TwoWay };
                        variable.SetBinding(VariableWrapper.ToolboxModeProperty, Binding); InputConnectors.Add(variable);
                        var ModeBinding = new Binding() { Path = new PropertyPath("DesignMode"), Source = this, Mode = BindingMode.TwoWay };
                        variable.SetBinding(VariableWrapper.DesignModeProperty, ModeBinding);
                        InputConnectors.Add(variable);
                    }
                }
            }
        }
        private void FillOutputConnectors()
        {
            if (Node.UseOutputs)
            {
                IC_OutputConnectors = GetTemplateChild("PART_OutputConnectors") as ItemsControl;
                IC_OutputConnectors.Visibility = Visibility.Visible;
                foreach (var item in Node.Outputs)
                {
                    if (item.VariableType == VariableType.Output)
                    {
                        var variable = new VariableWrapper(item);
                        var Binding = new Binding() { Path = new PropertyPath("ToolboxMode"), Source = this, Mode = BindingMode.TwoWay };
                        variable.SetBinding(VariableWrapper.ToolboxModeProperty, Binding);
                        var ModeBinding = new Binding() { Path = new PropertyPath("DesignMode"), Source = this, Mode = BindingMode.TwoWay };
                        variable.SetBinding(VariableWrapper.DesignModeProperty, ModeBinding);
                        OutputConnectors.Add(variable);
                    }
                }
            }
        }
        private void FillSettings()
        {
            if (Node.UseSettings)
            {
                IC_InternalPropertyItems = GetTemplateChild("PART_InternalPropertyItems") as ItemsControl;
                IC_InternalPropertyItems.Visibility = Visibility.Visible;

                foreach (var item in Node.Settings)
                {
                    if (item.VariableType == VariableType.Setting)
                    {
                        if (item.DataType == typeof(decimal))
                        {
                            var property = new DecimalBox() { CornerRadius = new CornerRadius(2), Height = 20, MinWidth = 60, HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(1, 0, 1, 2), Watermark = item.Name, VerticalAlignment = VerticalAlignment.Top, TextAlignment = TextAlignment.Right };
                            var Binding = new Binding() { Path = new PropertyPath("Value"), Source = item, Mode = BindingMode.TwoWay };
                            property.SetBinding(DecimalBox.ValueProperty, Binding);
                            InternalPropertyItems.Add(property);
                        }
                        else if (item.DataType == typeof(double))
                        {
                            var property = new DoubleBox() { CornerRadius = new CornerRadius(2), Height = 20, MinWidth = 60, HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(1, 0, 1, 2), Watermark = item.Name, VerticalAlignment = VerticalAlignment.Top, TextAlignment = TextAlignment.Right };
                            var Binding = new Binding() { Path = new PropertyPath("Value"), Source = item, Mode = BindingMode.TwoWay };
                            property.SetBinding(DoubleBox.ValueProperty, Binding);
                            InternalPropertyItems.Add(property);
                        }
                        else if (item.DataType == typeof(int))
                        {
                            var property = new IntegerBox() { CornerRadius = new CornerRadius(2), Height = 20, MinWidth = 60, HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(1, 0, 1, 2), Watermark = item.Name, VerticalAlignment = VerticalAlignment.Top, TextAlignment = TextAlignment.Right };
                            var Binding = new Binding() { Path = new PropertyPath("Value"), Source = item, Mode = BindingMode.TwoWay };
                            property.SetBinding(IntegerBox.ValueProperty, Binding);
                            InternalPropertyItems.Add(property);
                        }
                        else if (item.DataType == typeof(uint))
                        {
                            var property = new UIntegerBox() { CornerRadius = new CornerRadius(2), Minimum = 0, Height = 20, MinWidth = 60, HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(1, 0, 1, 2), Watermark = item.Name, VerticalAlignment = VerticalAlignment.Top, TextAlignment = TextAlignment.Right };
                            var Binding = new Binding() { Path = new PropertyPath("Value"), Source = item, Mode = BindingMode.TwoWay };
                            property.SetBinding(UIntegerBox.ValueProperty, Binding);
                            InternalPropertyItems.Add(property);
                        }
                        else if (item.DataType == typeof(bool))
                        {
                            var property = new UISwitch() { Height = 16, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(1, 0, 1, 2), Header = item.Name };
                            var Binding = new Binding() { Path = new PropertyPath("Value"), Source = item, Mode = BindingMode.TwoWay };
                            property.SetBinding(UISwitch.IsCheckedProperty, Binding);

                            InternalPropertyItems.Add(property);
                        }
                    }
                }
            }
        }

        private void NodeWrapper_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NodeWrapper_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Parent != null)
            {
                if (Parent is NodeCanvas)
                {
                    if ((Parent as NodeCanvas).GridOn)
                    {
                        if (IsSelected)
                        {
                            var SizeRect = NodeCanvasHelper.AdjustRectToGrid(new Rect(Canvas.GetLeft(this), Canvas.GetTop(this), ActualWidth, ActualHeight), (Parent as NodeCanvas).GridOn, (Parent as NodeCanvas).GridDelta);
                            Width = SizeRect.Width;
                            Height = SizeRect.Height;
                            if (Node != null)
                            {
                                Node.Size.Width = Width;
                                Node.Size.Height = Height;
                            }
                        }
                    }
                }
            }
        }
        
        public Guid ParentId
        {
            get => (Guid)GetValue(ParentIdProperty);
            set => SetValue(ParentIdProperty, value);
        }
        public string NodeName
        {
            get => (string)GetValue(NodeNameProperty);
            set => SetValue(NodeNameProperty, value);
        }
        public bool IsGroup
        {
            get => (bool)GetValue(IsGroupProperty);
            set => SetValue(IsGroupProperty, value);
        }
        public ObservableCollection<UIElement> InputConnectors
        {
            get => (ObservableCollection<UIElement>)GetValue(InputConnectorsProperty);
            set => SetValue(InputConnectorsProperty, value);
        }
        public ObservableCollection<UIElement> OutputConnectors
        {
            get => (ObservableCollection<UIElement>)GetValue(OutputConnectorsProperty);
            set => SetValue(OutputConnectorsProperty, value);
        }
        public ObservableCollection<UIElement> InternalPropertyItems
        {
            get => (ObservableCollection<UIElement>)GetValue(InternalPropertyItemsProperty);
            set => SetValue(InternalPropertyItemsProperty, value);
        }
        public bool ToolboxMode
        {
            get => (bool)GetValue(ToolboxModeProperty);
            set => SetValue(ToolboxModeProperty, value);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public bool IsDragConnectionOver
        {
            get { return (bool)GetValue(IsDragConnectionOverProperty); }
            set { SetValue(IsDragConnectionOverProperty, value); }
        }
        public NodeStyleEnum NodeStyle
        {
            get { return (NodeStyleEnum)GetValue(NodeStyleProperty); }
            set { SetValue(NodeStyleProperty, value); }
        }
        public bool ShowHeader
        {
            get => (bool)GetValue(ShowHeaderProperty);
            set => SetValue(ShowHeaderProperty, value);
        }
        public bool DesignMode
        {
            get => (bool)GetValue(DesignModeProperty);
            set => SetValue(DesignModeProperty, value);
        }
        public object Icon
        {
            get => (object)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty ParentIdProperty = DependencyProperty.Register("ParentId", typeof(Guid), typeof(NodeWrapper));
        public static readonly DependencyProperty NodeNameProperty = DependencyProperty.Register("NodeName", typeof(string), typeof(NodeWrapper), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IsGroupProperty = DependencyProperty.Register("IsGroup", typeof(bool), typeof(NodeWrapper), new UIPropertyMetadata(false));

        public static readonly DependencyProperty InputConnectorsProperty = DependencyProperty.Register("InputConnectors", typeof(ObservableCollection<UIElement>), typeof(NodeWrapper), new UIPropertyMetadata(null));
        public static readonly DependencyProperty OutputConnectorsProperty = DependencyProperty.Register("OutputConnectors", typeof(ObservableCollection<UIElement>), typeof(NodeWrapper), new UIPropertyMetadata(null));
        public static readonly DependencyProperty InternalPropertyItemsProperty = DependencyProperty.Register("InternalPropertyItems", typeof(ObservableCollection<UIElement>), typeof(NodeWrapper), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ToolboxModeProperty = DependencyProperty.Register("ToolboxMode", typeof(bool), typeof(NodeWrapper), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(NodeWrapper), new FrameworkPropertyMetadata(new CornerRadius(7)));
        public static readonly DependencyProperty IsDragConnectionOverProperty = DependencyProperty.Register("IsDragConnectionOver", typeof(bool), typeof(NodeWrapper), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty NodeStyleProperty = DependencyProperty.Register("NodeStyle", typeof(NodeStyleEnum), typeof(NodeWrapper), new FrameworkPropertyMetadata(NodeStyleEnum.Default));
        public static readonly DependencyProperty ShowHeaderProperty = DependencyProperty.Register("ShowHeader", typeof(bool), typeof(NodeWrapper), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty DesignModeProperty = DependencyProperty.Register("DesignMode", typeof(bool), typeof(NodeWrapper), new UIPropertyMetadata(true));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(NodeWrapper), new UIPropertyMetadata(null));

    }
}
