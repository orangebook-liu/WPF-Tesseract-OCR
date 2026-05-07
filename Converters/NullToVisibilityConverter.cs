using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OcrTool.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = parameter != null && parameter.ToString().Equals("invert", StringComparison.OrdinalIgnoreCase);
            
            if (value == null)
            {
                return invert ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return invert ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}