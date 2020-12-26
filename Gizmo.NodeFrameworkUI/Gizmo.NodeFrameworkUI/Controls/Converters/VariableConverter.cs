using Gizmo.NodeFramework;
using System;
using System.Windows.Data;

namespace Gizmo.NodeFrameworkUI
{
    public class VariableConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Variable variable = (Variable)value;
            string string_parameter = parameter as string;
            if (variable != null)
            {
                switch (string_parameter)
                {
                    case "Name":
                        {
                            return variable.Name;
                        }
                    case "Value":
                        {
                            return variable.Value;
                        }
                    default:
                        return null;
                }
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
