using System;
using System.Globalization;
using System.Windows.Data;

namespace Extensions.Wpf.Converters
{
    public class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (s == null) return null;
            Uri result;
            if (!Uri.TryCreate(s, UriKind.Absolute, out result)) return null;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uri = value as Uri;
            return uri != null ? uri.AbsoluteUri : null;
        }
    }
}