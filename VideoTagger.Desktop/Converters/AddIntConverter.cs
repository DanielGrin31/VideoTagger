using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace VideoTagger.Desktop.Converters;

public class AddIntConverter:IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int num)
        {
            if (parameter is not null&&int.TryParse(parameter.ToString(), out int num2))
            {
                return num + num2;
            }
            else
            {
                return num;
            }
        }
        return new BindingNotification(new InvalidCastException(),
            BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}