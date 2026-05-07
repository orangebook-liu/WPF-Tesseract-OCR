using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OcrTool.Converters
{
    public class DragOverBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDragOver = value is bool && (bool)value;
            return isDragOver 
                ? new SolidColorBrush(Color.FromRgb(0x4A, 0x90, 0xD9)) 
                : new SolidColorBrush(Color.FromRgb(0x4A, 0x90, 0xD9));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}