using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace BeghShare.UI.Converters
{
    public class BoolToVisibilityConverter : AutoConverter<BoolToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value is bool b && b;

            // Check if parameter is "inverse"
            if (parameter is string str && str.Equals("inverse", StringComparison.OrdinalIgnoreCase))
                flag = !flag;

            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value is Visibility v && v == Visibility.Visible;

            if (parameter is string str && str.Equals("inverse", StringComparison.OrdinalIgnoreCase))
                result = !result;

            return result;
        }
    }

}
