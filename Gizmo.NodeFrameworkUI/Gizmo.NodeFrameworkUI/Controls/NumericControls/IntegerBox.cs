using Gizmo.WPF;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Gizmo.NodeFrameworkUI
{
    [TemplatePart(Name = PART_UpButton, Type = typeof(UIRepeatButton))]
    [TemplatePart(Name = PART_DownButton, Type = typeof(UIRepeatButton))]
    public class IntegerBox : UITextBox
    {
        private const string PART_UpButton = "PART_UpButton";
        private const string PART_DownButton = "PART_DownButton";

        public UIRepeatButton DownButton;

        public UIRepeatButton UpButton;
        static IntegerBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerBox), new FrameworkPropertyMetadata(typeof(IntegerBox)));
        }
        public IntegerBox() : base()
        {
            DefaultStyleKey = typeof(IntegerBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PreviewKeyDown += OnPreviewKeyDown;
            Text = Value.ToString();

            MouseWheel += RollMouseWheel;
            UpButton = GetTemplateChild(PART_UpButton) as UIRepeatButton;
            DownButton = GetTemplateChild(PART_DownButton) as UIRepeatButton;

            if (ShowControlButtons)
                UpdateCornerRadius();

            if (UpButton != null)
            {
                UpButton.Click -= UpButton_Click;
                UpButton.Click += UpButton_Click;
            }
            if (DownButton != null)
            {
                DownButton.Click -= DownButton_Click;
                DownButton.Click += DownButton_Click;
            }

            CanMoveUp = (Value + Increment) <= Maximum;
            CanMoveDown = (Value - Increment) >= Minimum;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    {
                        Value = IncrementValue(Value, Increment);
                        e.Handled = true;
                        break;
                    }
                case Key.Down:
                    {
                        Value = DecrementValue(Value, Increment);
                        e.Handled = true;
                        break;
                    }
                case Key.Enter:
                    {
                        Value = ConvertTextToValue(Text).Value;
                        e.Handled = true;
                        break;
                    }
            }
        }

        private void RollMouseWheel(object sender, MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!e.Handled)
            {
                if (e.Delta != 0)
                {
                    if (e.Delta < 0)
                    {
                        CanMoveUp = (Value + Increment) <= Maximum;
                        CanMoveDown = (Value - Increment) >= Minimum;
                        if (CanMoveDown)
                            DoDecrement();
                        CanMoveUp = (Value + Increment) <= Maximum;
                        CanMoveDown = (Value - Increment) >= Minimum;
                    }
                    else
                    {
                        CanMoveUp = (Value + Increment) <= Maximum;
                        CanMoveDown = (Value - Increment) >= Minimum;
                        if (CanMoveUp)
                            DoIncrement();
                        CanMoveUp = (Value + Increment) <= Maximum;
                        CanMoveDown = (Value - Increment) >= Minimum;
                    }
                }
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                CanMoveUp = (Value + Increment) <= Maximum;
                CanMoveDown = (Value - Increment) >= Minimum;
                if (CanMoveDown)
                    DoDecrement();
                CanMoveUp = (Value + Increment) <= Maximum;
                CanMoveDown = (Value - Increment) >= Minimum;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                CanMoveUp = (Value + Increment) <= Maximum;
                CanMoveDown = (Value - Increment) >= Minimum;
                if (CanMoveUp)
                    DoIncrement();
                CanMoveUp = (Value + Increment) <= Maximum;
                CanMoveDown = (Value - Increment) >= Minimum;
            }
        }

        private void UpdateCornerRadius()
        {
            if (UpButton != null)
            {
                UpButton.CornerRadius = new CornerRadius(0d, CornerRadius.TopRight, 0d, 0d);
            }
            if (DownButton != null)
            {
                DownButton.CornerRadius = new CornerRadius(0d, 0d, CornerRadius.BottomRight, 0d);
            }        }

        #region Protected Methods
        protected int IncrementValue(int value, int increment)
        {
            return value + increment;
        }

        protected int DecrementValue(int value, int increment)
        {
            return value - increment;
        }

        protected int? ConvertTextToValue(string text)
        {
            int? result = null;

            if (string.IsNullOrEmpty(text))
                return result;

            string currentValueText = ConvertValueToText();
            if (Equals(currentValueText, text))
            {
                IsValidationError = false;
                ValidationError = null;
                return Value;
            }

            result = TextToValue(currentValueText, text);
            ValidateDefaultMinMax(result);
            return result;
        }

        protected string ConvertValueToText()
        {
            IsValidationError = false;
            ValidationError = null;

            if (FormatString.Contains("{0"))
                return string.Format(CultureInfo, FormatString, Value);

            return Value.ToString(FormatString, CultureInfo);
        }

        protected int? TextToValue(string currentText, string text)
        {
            int? result;
            int outputValue = new int();
            if (!int.TryParse(text, NumberStyle, CultureInfo, out outputValue))
            {
                bool ShowValidationError = true;

                if (!int.TryParse(currentText, NumberStyle, CultureInfo, out int currentValueTextOutputValue))
                {
                    var isSpecialValue = currentText.Where(c => !char.IsDigit(c));
                    if (isSpecialValue.Any())
                    {
                        var isTextChecialChars = text.Where(c => !char.IsDigit(c));
                        if (isSpecialValue.Except(isTextChecialChars).ToList().Count == 0)
                        {
                            foreach (var character in isTextChecialChars)
                            {
                                text = text.Replace(character.ToString(), string.Empty);
                            }
                            if (int.TryParse(text, NumberStyle, CultureInfo, out outputValue))
                            {
                                ShowValidationError = false;
                            }
                        }
                    }
                }

                if (ShowValidationError)
                {
                    IsValidationError = true;
                    ValidationError = "Value is not in a correct format.";
                }
                else
                {
                    IsValidationError = false;
                    ValidationError = null;
                }
            }
            result = outputValue;
            return result;
        }

        protected void ValidateDefaultMinMax(int? value)
        {
            if (Equals(value, DefaultValue))
            {
                IsValidationError = false;
                ValidationError = null;
                return;
            }
            if (IsLowerThan(value, Minimum))
            {
                IsValidationError = true;
                ValidationError = string.Format("Value must be greater than Minimum Value - {0}", Minimum);
            }
            else if (IsGreaterThan(value, Maximum))
            {
                IsValidationError = true;
                ValidationError = string.Format("Value must be less than Maximum Value - {0}", Maximum);
            }
        }

        private bool IsLowerThan(int? first, int? second)
        {
            if (first == null || second == null)
                return false;

            return first.Value < second.Value;
        }

        private bool IsGreaterThan(int? first, int? second)
        {
            if (first == null || second == null)
                return false;

            return first.Value > second.Value;
        }


        #endregion

        #region Public Methods
        public void DoIncrement()
        {
            Value = IncrementValue(Value, Increment);
        }

        public void DoDecrement()
        {
            Value = DecrementValue(Value, Increment);
        }
        #endregion

        #region Public Properties
        public int Minimum
        {
            get => (int)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        public int Increment
        {
            get => (int)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public int DefaultValue
        {
            get => (int)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        public CultureInfo CultureInfo
        {
            get => (CultureInfo)GetValue(CultureInfoProperty);
            set => SetValue(CultureInfoProperty, value);
        }

        public string FormatString
        {
            get => (string)GetValue(FormatStringProperty);
            set => SetValue(FormatStringProperty, value);
        }

        public bool UpdateValueOnEnterKey
        {
            get => (bool)GetValue(UpdateValueOnEnterKeyProperty);
            set => SetValue(UpdateValueOnEnterKeyProperty, value);
        }
        public NumberStyles NumberStyle
        {
            get => (NumberStyles)GetValue(NumberStyleProperty);
            set => SetValue(NumberStyleProperty, value);
        }
        public bool ShowControlButtons
        {
            get => (bool)GetValue(ShowControlButtonsProperty);
            set => SetValue(ShowControlButtonsProperty, value);
        }
        public bool CanMoveUp
        {
            get => (bool)GetValue(CanMoveUpProperty);
            set => SetValue(CanMoveUpProperty, value);
        }
        public bool CanMoveDown
        {
            get => (bool)GetValue(CanMoveDownProperty);
            set => SetValue(CanMoveDownProperty, value);
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(int), typeof(IntegerBox), new UIPropertyMetadata(int.MinValue, MinimumPropertyChanged));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(IntegerBox), new UIPropertyMetadata(int.MaxValue, MaximumPropertyChanged));
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(int), typeof(IntegerBox), new UIPropertyMetadata(1, IncrementPropertyChanged));
        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(int), typeof(IntegerBox), new UIPropertyMetadata(default(int)));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(IntegerBox), new UIPropertyMetadata(0, ValuePropertyChanged));
        public static readonly DependencyProperty CultureInfoProperty = DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(IntegerBox), new UIPropertyMetadata(CultureInfo.CurrentCulture, OnCultureInfoChanged));
        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register("FormatString", typeof(string), typeof(IntegerBox), new UIPropertyMetadata(String.Empty, OnFormatStringChanged));
        public static readonly DependencyProperty UpdateValueOnEnterKeyProperty = DependencyProperty.Register("UpdateValueOnEnterKey", typeof(bool), typeof(IntegerBox), new FrameworkPropertyMetadata(false, OnUpdateValueOnEnterKeyChanged));
        public static readonly DependencyProperty NumberStyleProperty = DependencyProperty.Register("NumberStyle", typeof(NumberStyles), typeof(IntegerBox), new UIPropertyMetadata(NumberStyles.Any));
        public static readonly DependencyProperty ShowControlButtonsProperty = DependencyProperty.Register("ShowControlButtons", typeof(bool), typeof(IntegerBox), new UIPropertyMetadata(false, ShowControlButtonsPropertyChanged));
        public static readonly DependencyProperty CanMoveUpProperty = DependencyProperty.Register("CanMoveUp", typeof(bool), typeof(IntegerBox), new UIPropertyMetadata(true));
        public static readonly DependencyProperty CanMoveDownProperty = DependencyProperty.Register("CanMoveDown", typeof(bool), typeof(IntegerBox), new UIPropertyMetadata(true));
        #endregion

        #region Dependency Properties Callbacks
        private static void MinimumPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is IntegerBox TextBox)
                TextBox.OnMinimumPropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected void OnMinimumPropertyChanged(int oldValue, int newValue)
        { }

        private static void MaximumPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is IntegerBox TextBox)
                TextBox.OnMaximumPropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected void OnMaximumPropertyChanged(int oldValue, int newValue)
        { }

        private static void IncrementPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is IntegerBox TextBox)
                TextBox.OnIncrementPropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected void OnIncrementPropertyChanged(int oldValue, int newValue)
        { }

        private static void ValuePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is IntegerBox TextBox)
                TextBox.OnValuePropertyChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected void OnValuePropertyChanged(int oldValue, int newValue)
        {
            if (newValue > Maximum)
            {
                Value = Maximum;
            }
            else if (newValue < Minimum)
            {
                Value = Minimum;
            }
            Text = Value.ToString();
        }

        private static void OnCultureInfoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is IntegerBox TextBox)
                TextBox.OnCultureInfoChanged((CultureInfo)e.OldValue, (CultureInfo)e.NewValue);
        }

        protected void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
        { }

        private static void OnFormatStringChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is IntegerBox TextBox)
                TextBox.OnFormatStringChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected void OnFormatStringChanged(string oldValue, string newValue)
        { }

        private static void OnUpdateValueOnEnterKeyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is IntegerBox TextBox)
                TextBox.OnUpdateValueOnEnterKeyChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected void OnUpdateValueOnEnterKeyChanged(bool oldValue, bool newValue)
        { }

        private static void ShowControlButtonsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IntegerBox source = d as IntegerBox;
            source.OnShowControlButtonsPropertyChanged();
        }

        protected virtual void OnShowControlButtonsPropertyChanged()
        {
            UpdateCornerRadius();
        }

        #endregion
    }
}
