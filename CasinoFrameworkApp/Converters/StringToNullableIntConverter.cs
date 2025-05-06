using System;
using System.Globalization;
using System.Windows.Data;

namespace CasinoFrameworkApp.Converters
{
    public class StringToNullableIntConverter : IValueConverter
    {
        // int? -> string
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value?.ToString() ?? string.Empty;

        // string -> int?
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (string.IsNullOrWhiteSpace(s))
                return null;
            return int.TryParse(s, out var i) ? (int?)i : Binding.DoNothing;
        }
    }
}
