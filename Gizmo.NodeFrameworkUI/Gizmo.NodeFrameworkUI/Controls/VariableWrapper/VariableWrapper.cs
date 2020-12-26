using Gizmo.NodeFramework;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Gizmo.NodeFrameworkUI
{
    public class VariableWrapper : ContentControl, INotifyPropertyChanged
    {
        #region Private Properties
        private Border ConnectionPointBorder;
        private List<LinkWrapper> links;
        private Point? dragStartPoint = null;
        private Point position;
        private NodeWrapper parentNodeWrapper;
        #endregion

        #region Public Properties
        public Point Position
        {
            get => position;
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged("Position");
                }
            }
        }
        public List<LinkWrapper> Links
        {
            get
            {
                if (links == null)
                    links = new List<LinkWrapper>();
                return links;
            }
        }
        public NodeWrapper ParentNodeWrapper
        {
            get
            {
                if (parentNodeWrapper == null)
                    parentNodeWrapper = DataContext as NodeWrapper;

                return parentNodeWrapper;
            }
        }
        public bool IsConnected
        {
            get => (bool)GetValue(IsConnectedProperty);
            set => SetValue(IsConnectedProperty, value);
        }
        public bool ToolboxMode
        {
            get => (bool)GetValue(ToolboxModeProperty);
            set => SetValue(ToolboxModeProperty, value);
        }
        public VariableWrapperOrientation Orientation
        {
            get => (VariableWrapperOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        public Variable Variable
        {
            get => (Variable)GetValue(VariableProperty);
            set => SetValue(VariableProperty, value);
        }
        public bool ShowValue
        {
            get => (bool)GetValue(ShowValueProperty);
            set => SetValue(ShowValueProperty, value);
        }
        public bool ShowName
        {
            get => (bool)GetValue(ShowNameProperty);
            set => SetValue(ShowNameProperty, value);
        }
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }
        public bool DesignMode
        {
            get => (bool)GetValue(DesignModeProperty);
            set => SetValue(DesignModeProperty, value);
        }
        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty IsConnectedProperty = DependencyProperty.Register("IsConnected", typeof(bool), typeof(VariableWrapper), new UIPropertyMetadata(false));
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(VariableWrapperOrientation), typeof(VariableWrapper));
        public static readonly DependencyProperty ToolboxModeProperty = DependencyProperty.Register("ToolboxMode", typeof(bool), typeof(VariableWrapper), new UIPropertyMetadata(false));
        public static readonly DependencyProperty VariableProperty = DependencyProperty.Register("Variable", typeof(Variable), typeof(VariableWrapper), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ShowNameProperty = DependencyProperty.Register("ShowName", typeof(bool), typeof(VariableWrapper), new UIPropertyMetadata(true));
        public static readonly DependencyProperty ShowValueProperty = DependencyProperty.Register("ShowValue", typeof(bool), typeof(VariableWrapper), new UIPropertyMetadata(true));
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(VariableWrapper), new UIPropertyMetadata(false));
        public static readonly DependencyProperty DesignModeProperty = DependencyProperty.Register("DesignMode", typeof(bool), typeof(VariableWrapper), new UIPropertyMetadata(true));
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region VariableWrapper Constructors
        public VariableWrapper()
: base()
        {
            DefaultStyleKey = typeof(VariableWrapper);
        }
        public VariableWrapper(Variable variable)
: base()
        {
            InitVariableWrapper(variable);
            DefaultStyleKey = typeof(VariableWrapper);
        }
        #endregion

        #region Override Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ApplyVisuals();
        }

        private void ApplyVisuals()
        {
            ConnectionPointBorder = GetTemplateChild("PART_ConnectionPointBorder") as Border;
            LayoutUpdated += VariableWrapper_LayoutUpdated;
            if (Variable != null)
            {
                if (Variable.IsEditable)
                {
                    if (Variable.DataType == typeof(decimal))
                    {
                        ShowName = false;
                        var property = new DecimalBox() { CornerRadius = new CornerRadius(4), Height = 16, MinWidth = 90, HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(1, 1, 4, 1), VerticalAlignment = VerticalAlignment.Center, Padding = new Thickness(), TextAlignment = TextAlignment.Right };
                        var ValueBinding = new Binding() { Path = new PropertyPath("Value"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(DecimalBox.ValueProperty, ValueBinding);
                        var NameBinding = new Binding() { Path = new PropertyPath("Name"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(DecimalBox.WatermarkProperty, NameBinding);
                        Content = property;
                    }
                    else if (Variable.DataType == typeof(double))
                    {
                        ShowName = false;
                        var property = new DoubleBox() { CornerRadius = new CornerRadius(4), Height = 16, MinWidth = 90, HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(1, 1, 4, 1), VerticalAlignment = VerticalAlignment.Center, Padding = new Thickness(), TextAlignment = TextAlignment.Right };
                        var ValueBinding = new Binding() { Path = new PropertyPath("Value"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(DoubleBox.ValueProperty, ValueBinding);
                        var NameBinding = new Binding() { Path = new PropertyPath("Name"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(DoubleBox.WatermarkProperty, NameBinding);
                        Content = property;
                    }
                    else if (Variable.DataType == typeof(int))
                    {
                        ShowName = false;
                        var property = new IntegerBox() { CornerRadius = new CornerRadius(4), Height = 16, MinWidth = 90, HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(1, 1, 4, 1), VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Right };
                        var ValueBinding = new Binding() { Path = new PropertyPath("Value"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(IntegerBox.ValueProperty, ValueBinding);
                        var NameBinding = new Binding() { Path = new PropertyPath("Name"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(IntegerBox.WatermarkProperty, NameBinding);
                        Content = property;
                    }
                    else if (Variable.DataType == typeof(uint))
                    {
                        ShowName = false;
                        var property = new UIntegerBox() { CornerRadius = new CornerRadius(4), Height = 16, MinWidth = 90, HorizontalAlignment = HorizontalAlignment.Stretch, Margin = new Thickness(1, 1, 4, 1), VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Right };
                        var ValueBinding = new Binding() { Path = new PropertyPath("Value"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(UIntegerBox.ValueProperty, ValueBinding);
                        var NameBinding = new Binding() { Path = new PropertyPath("Name"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(UIntegerBox.WatermarkProperty, NameBinding);
                        Content = property;
                    }
                    else if (Variable.DataType == typeof(bool))
                    {
                        ShowName = false;
                        var property = new UISwitch() { Height = 16, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center, CornerRadius = new CornerRadius(8), Margin = new Thickness(1, 1, 4, 1) };
                        var ValueBinding = new Binding() { Path = new PropertyPath("Value"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(UISwitch.IsCheckedProperty, ValueBinding);
                        var NameBinding = new Binding() { Path = new PropertyPath("Name"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(UISwitch.HeaderProperty, NameBinding);
                        Content = property;
                    }
                    else if (Variable.DataType == typeof(string))
                    {
                        ShowName = false;
                        var property = new UITextBox() { CornerRadius = new CornerRadius(4), Height = 16, MinWidth = 90, HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center, Padding = new Thickness(), Margin = new Thickness(1, 1, 4, 1), TextAlignment = TextAlignment.Right };
                        var ValueBinding = new Binding() { Path = new PropertyPath("Value"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(UITextBox.TextProperty, ValueBinding);
                        var NameBinding = new Binding() { Path = new PropertyPath("Name"), Source = Variable, Mode = BindingMode.TwoWay };
                        property.SetBinding(UITextBox.WatermarkProperty, NameBinding); Content = property;
                    }
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (VisualHelper.FindParent<NodeCanvas>(this) is NodeCanvas nodeDesigner)
            {
                if (nodeDesigner.DesignMode && DesignMode)
                {
                    dragStartPoint = new Point?(e.GetPosition(nodeDesigner));
                    e.Handled = true;
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
                dragStartPoint = null;

            if (dragStartPoint.HasValue)
            {
                if (VisualHelper.FindParent<NodeCanvas>(this) is NodeCanvas nodeDesigner)
                {
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(nodeDesigner);
                    if (adornerLayer != null)
                    {
                        VariableWrapperAdorner adorner = new VariableWrapperAdorner(nodeDesigner as NodeCanvas, this);
                        if (adorner != null)
                        {
                            adornerLayer.Add(adorner);
                            e.Handled = true;
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void InitVariableWrapper(Variable variable)
        {
            Variable = variable;
            ShowName = Variable.ShowName;
            IsEditable = Variable.IsEditable;
            ShowValue = Variable.ShowValue;

            if (Variable.VariableType == VariableType.Input)
            {
                Orientation = VariableWrapperOrientation.Left;
            }
            else if (Variable.VariableType == VariableType.Output)
            {
                Orientation = VariableWrapperOrientation.Right;
            }
        }

        private void VariableWrapper_LayoutUpdated(object sender, EventArgs e)
        {
            if (VisualHelper.FindParent<NodeCanvas>(this) is NodeCanvas nodeDesigner)
            {
                if (Orientation == VariableWrapperOrientation.Left)
                {
                    Position = TransformToAncestor(nodeDesigner).Transform(new Point(ConnectionPointBorder.Margin.Left + ConnectionPointBorder.ActualWidth / 2, ConnectionPointBorder.Margin.Top + ConnectionPointBorder.ActualHeight / 2));
                }
                else if (Orientation == VariableWrapperOrientation.Right)
                {
                    Position = TransformToAncestor(nodeDesigner).Transform(new Point(ActualWidth - (ConnectionPointBorder.Margin.Right + ConnectionPointBorder.ActualWidth / 2), ConnectionPointBorder.Margin.Top + ConnectionPointBorder.ActualHeight / 2));
                }
            }
        }

        #endregion

        #region Internal Methods
        internal VariableWrapperInfo GetInfo()
        {
            return new VariableWrapperInfo
            {
                Left = Canvas.GetLeft(ParentNodeWrapper),
                Top = Canvas.GetTop(ParentNodeWrapper),
                Size = new Size(ParentNodeWrapper.ActualWidth, ParentNodeWrapper.ActualHeight),
                Orientation = Orientation,
                Position = Position
            };
        }
        #endregion
    }
}
