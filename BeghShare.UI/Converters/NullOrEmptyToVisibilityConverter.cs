using System.Globalization;
using System.Windows;

namespace BeghShare.UI.Converters
{
    class NullOrEmptyToVisibilityConverter : AutoConverter<NullOrEmptyToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNullOrEmpty = value == null || (value is string s && string.IsNullOrWhiteSpace(s));

            // Optional inverse
            if (parameter is string str && str.Equals("inverse", StringComparison.OrdinalIgnoreCase))
                isNullOrEmpty = !isNullOrEmpty;

            return isNullOrEmpty ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
