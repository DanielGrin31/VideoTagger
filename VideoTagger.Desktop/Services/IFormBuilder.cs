using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace VideoTagger.Desktop.Services
{
    public interface IFormBuilder
    {
        List<Control> BuildForm(Dictionary<string, string> Fields,
         EventHandler<RoutedEventArgs> submitAction);
    }
}