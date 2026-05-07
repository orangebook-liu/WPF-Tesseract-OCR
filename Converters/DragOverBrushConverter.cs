using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OcrTool.Converters
{
    public class DragOverBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDragOver = value is bool && (bool)value;
            return isDragOver 
                ? new SolidColorBrush(Color.FromArgb(0x20, 0x4A, 0x90, 0xD9)) 
                : new SolidColorBrush(Color.FromArgb(0xFF, 0xF5, 0xF8, 0xFC));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}