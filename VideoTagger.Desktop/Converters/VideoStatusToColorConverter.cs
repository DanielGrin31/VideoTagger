using System;
using System.Globalization;

using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Converters
{
    public class VideoStatusToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ReviewStatus status)
            {
                var brush=status switch
                {
                    ReviewStatus.Horror => Brushes.IndianRed,
                    ReviewStatus.Seen => Brushes.LightBlue,
                    ReviewStatus.NotSeen => Brushes.White,
                    _ => throw new NotImplementedException()
                };
                return brush;
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