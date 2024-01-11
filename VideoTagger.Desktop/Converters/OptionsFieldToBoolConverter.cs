using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Data;
using Avalonia.Data.Converters;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Converters
{
    public class OptionsFieldToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is FormFieldType fieldType)
            {
                switch (fieldType)
                {
                    case FormFieldType.TextBox:
                        return false;

                    case FormFieldType.CheckBox:
                    case FormFieldType.ComboBox:
                        return true;
                    default:
                        return false;
                        // invalid option, return the exception below
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
}