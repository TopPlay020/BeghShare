using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BeghShare.UI.Converters
{
    public abstract class AutoConverter<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        private static T? _instance;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ??= new T();
        }

        public abstract object? Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
