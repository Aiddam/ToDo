using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ToDo.Converters
{
    public class ComboBoxConverter : IValueConverter
    {
        private readonly IValueConverter _valueConverter;

        public ComboBoxConverter(IValueConverter valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return new ComboBoxItem
            {
                Content = _valueConverter.Convert(value, targetType, parameter, culture)
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var comboBoxItem = value as ComboBoxItem;
            if (comboBoxItem is null) return null;
            return _valueConverter.ConvertBack(comboBoxItem.Content, targetType, parameter, culture);
        }
    }
}
