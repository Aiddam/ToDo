using Avalonia.Data.Converters;
using ToDo.Models.Models;
using System;
using System.Globalization;

namespace ToDo.Converters
{
    public class LoginButtonConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var user = value as User;
            if (user is null) return false;
            return true;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
