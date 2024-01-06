using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using VideoTagger.Desktop.ViewModels;
using VideoTagger.Desktop.Views;

namespace VideoTagger.Desktop;

public class ViewLocator : IDataTemplate
{
       private Dictionary<Type, Func<Control?>> _locator = new();

    public ViewLocator()
    {
        RegisterViewFactory();
    }

    public Control Build(object? data)
    {
        if (data is null)
            return new TextBlock { Text = $"No VM provided" };

        _locator.TryGetValue(data.GetType(), out var factory);

        return factory?.Invoke() ?? new TextBlock { Text = $"VM Not Registered: {data.GetType()}" };
    }

    public bool Match(object? data)
    {
        return data is ObservableObject;
    }

    public void RegisterViewFactory<TViewModel>(Func<Control> factory) where TViewModel : class => _locator.Add(typeof(TViewModel), factory);

    public void RegisterViewFactory<TViewModel, TView>()
        where TViewModel : class
        where TView : Control
        => _locator.Add(typeof(TViewModel), Ioc.Default.GetService<TView>);
    public void RegisterViewFactory()
    {
        var viewModelTypes = AppDomain.CurrentDomain.GetAssemblies()
       .SelectMany(s => s.GetTypes())
       .Where(p => typeof(ViewModelBase).IsAssignableFrom(p) && !p.IsInterface
       && !p.IsAbstract && p.Name.EndsWith("ViewModel"));
        foreach (var vmType in viewModelTypes)
        {
            var viewTypeName=vmType.Name.Substring(0,vmType.Name.Length-5);
            var viewType=Type.GetType("VideoTagger.Desktop.Views." + viewTypeName);
            if (viewType != null)
            {
                _locator.Add(vmType,()=> (Control?)Ioc.Default.GetService(viewType));
            }
        }
    }
}
