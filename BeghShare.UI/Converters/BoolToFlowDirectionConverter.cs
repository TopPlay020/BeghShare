using System;
using System.Globalization;
using System.Windows;

namespace BeghShare.UI.Converters
{
    public class BoolToFlowDirectionConverter : AutoConverter<BoolToFlowDirectionConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((FlowDirection)value) == FlowDirection.RightToLeft;
        }
    }
}
