namespace TfsWorkspaceUpdater.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    public class BoolToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool) value;
            var param = parameter as string;
            if ("invert".Equals(param))
                val = !val;

            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}