using System;
using System.Globalization;
using System.Windows.Data;

namespace AVSSalesExplorer.Common
{
    public class NullableNumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return string.Empty;
            }

            return value.ToString();            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return null;
            }

            var s = value.ToString();
            if (decimal.TryParse(s, out var result))
            {
                return result;
            }
            
            return null;
        }
    }
}