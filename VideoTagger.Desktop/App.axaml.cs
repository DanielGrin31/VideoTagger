using System;
using System.Linq;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using VideoTagger.Desktop.Services;
using VideoTagger.Desktop.Services.Forms;
using VideoTagger.Desktop.Services.Repositories;
using VideoTagger.Desktop.ViewModels;

namespace VideoTagger.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    public override void OnFrameworkInitializationCompleted()
    {
        var locator = new ViewLocator();
        DataTemplates.Add(locator);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            BindingPlugins.DataValidators.RemoveAt(0);

            var services = new ServiceCollection();
            // You can split registrations across multiple methods or classes, but you need to remember to call them all
            ConfigureServices(services);

            var provider = services.BuildServiceProvider(); // Warning in MEDI 7.0, fixed in 8.0

            Ioc.Default.ConfigureServices(provider);

            var vm = Ioc.Default.GetService<ShellViewModel>();
            var view = (Window)locator.Build(vm);
            view.DataContext = vm;

            desktop.MainWindow = view;
        }

        base.OnFrameworkInitializationCompleted();
    }

    internal static void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(App).Assembly);
        });
        services.AddSingleton<IVideoRepository, VideoRepository>();
        services.AddTransient<VideoLoader>();
        services.AddSingleton<IFileHasher,MD5FileHasher>();
        services.AddSingleton<IFormManager, FileFormManager>();
        services.AddTransient<IFormBuilder, FormBuilder>();
        services.AddTransient<IFormExporter, CsvFormExporter>();
        RegisterViews(services);
    }
    internal static void RegisterViews(IServiceCollection services)
    {
        var viewModelTypes = AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(s => s.GetTypes())
               .Where(p => typeof(ViewModelBase).IsAssignableFrom(p) && !p.IsInterface
               && !p.IsAbstract && p.Name.EndsWith("ViewModel"));

        var viewTypes = AppDomain.CurrentDomain.GetAssemblies()
       .SelectMany(s => s.GetTypes())
       .Where(p => p.Name.EndsWith("View") && p.Namespace!.StartsWith("VideoTagger.Desktop.Views"));


        foreach (Type viewType in viewTypes)
        {
            services.AddTransient(viewType);
        }
        foreach (Type vmType in viewModelTypes)
        {
            services.AddTransient(vmType);
        }
    }


}