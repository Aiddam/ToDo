using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ToDo.Converters
{
    public class DateConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var date = value as TimeSpan?;
            if (date is null) return null;

            return $"{(date.Value.Hours > 10 ? date.Value.Hours : "0" + date.Value.Hours)}:{(date.Value.Minutes > 10 ? date.Value.Minutes : "0" + date.Value.Minutes)}";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str is null) return null;

            var splittedStr = str.Split(':');
            return new TimeSpan(int.Parse(splittedStr[0]), int.Parse(splittedStr[1]), 0);
        }
    }
}
