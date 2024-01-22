using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace VideoTagger.Desktop.Converters;

public class StringsMatchToVisibileConverter :IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!values.All(obj=>obj is string))
        {
            return false;
        }
        var strings=values.Cast<string>().ToArray();
        if (strings.Length == 0)
        {
            return false;
        }
        return strings.All(str => str == strings[0]);
    }
}