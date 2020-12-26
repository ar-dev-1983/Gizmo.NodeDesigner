using System;
using System.Windows;
using System.Windows.Data;

namespace Gizmo.NodeFrameworkUI
{
    public class VariableWrapperOrientationToTextAlignmentConverter : IValueConverter
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
                            return TextAlignment.Right;
                        else if (s == "HO")
                            return TextAlignment.Left;
                        else
                            return TextAlignment.Center;
                    }

                case VariableWrapperOrientation.Right:
                    {
                        if (s == "H")
                            return TextAlignment.Left;
                        else if (s == "HO")
                            return TextAlignment.Right;
                        else
                            return TextAlignment.Center;
                    }
                case VariableWrapperOrientation.None:
                    return TextAlignment.Center;
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
