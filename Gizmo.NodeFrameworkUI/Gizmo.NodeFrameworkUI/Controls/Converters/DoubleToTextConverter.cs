using System;
using System.Windows.Data;

namespace Gizmo.NodeFrameworkUI
{
    public class DoubleToTextConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double.TryParse(value.ToString(), out double result);
            if (result == double.NaN)
            {
                return "NAN";
            }
            else if (result == double.NegativeInfinity)
            {
                return "-INF";
            }
            else if (result == double.PositiveInfinity)
            {
                return "+INF";
            }
            else if (result == double.MaxValue)
            {
                return "dMAX";
            }
            else if (result == double.MinValue)
            {
                return "dMIN";
            }
            else
            {
                return Math.Round(result, 2).ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
