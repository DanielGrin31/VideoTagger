using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Services
{
    public interface IFormBuilder
    {
        List<Control> BuildForm(IEnumerable<FormField> Fields,
         EventHandler<RoutedEventArgs> submitAction);
    }
}