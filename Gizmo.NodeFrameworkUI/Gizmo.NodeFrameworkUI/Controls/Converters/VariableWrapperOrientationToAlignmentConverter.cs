using System;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.NodeFrameworkUI
{
    public class VariableWrapperOrientationToAlignmentConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            VariableWrapperOrientation pointOrientation = (VariableWrapperOrientation)value;
            string s = parameter as string;
            switch (pointOrientation)
            {
                case VariableWrapperOrientation.Left:
                    {
                        if (s == "H")
                            return HorizontalAlignment.Left;
                        else if (s == "HO")
                            return HorizontalAlignment.Right;
                        else
                            return VerticalAlignment.Center;
                    }
                case VariableWrapperOrientation.Right:
                    {
                        if (s == "H")
                            return HorizontalAlignment.Right;
                        else if (s == "HO")
                            return HorizontalAlignment.Left;
                        else
                            return VerticalAlignment.Center;
                    }
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
