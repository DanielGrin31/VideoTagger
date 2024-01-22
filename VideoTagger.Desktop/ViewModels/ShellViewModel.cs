
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Views;

namespace VideoTagger.Desktop.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    private string videoDirectory;
    [ObservableProperty]
    private ViewModelBase _content;
    private readonly IMediator mediator;

  
    public ShellViewModel(MainViewModel mainViewModel, IMediator mediator)
    {
        this.mediator = mediator;
        Content = mainViewModel;
        mainViewModel.Parent = this;
    }
    public void NavigateTo<T>(Action<ViewModelBase>? postNavigationAction = null) where T : ViewModelBase
    {
        ViewModelBase vm=this.Build<T>();
        Content = vm;
        postNavigationAction?.Invoke(vm);
    }
    [RelayCommand]
    private async Task OpenFormConfig()
    {
        var desktop = App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
        // Start async operation to open the dialog.
        var fileDialogResult = await topLevel.StorageProvider
            .OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Select Form Config",
                AllowMultiple = false,
                FileTypeFilter = new []{FilePickerFileTypes.TextPlain}
            });
        var file = fileDialogResult.FirstOrDefault();
        if (file is null)
        {
            return;
        }

        string filePath=file.Path.AbsolutePath;
        FormConfigChangedMessage message = new FormConfigChangedMessage()
        {
            ConfigFilePath = filePath,
        };
        await mediator.Publish(message);
    }
    [RelayCommand]
    private void NavigateMain()
    {
        NavigateTo<MainViewModel>();
    }
    [RelayCommand]
    private async Task OpenVideoFolder()
    {
        var desktop = App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
        // Start async operation to open the dialog.
        var folderDialogResult = await topLevel.StorageProvider
        .OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Folder for videos",
            AllowMultiple = false
        });
        var folder = folderDialogResult.FirstOrDefault();
        if (folder is null)
        {
            return;
        }
        VideoDirectoryChangedMessage message = new VideoDirectoryChangedMessage()
        {
            OldVideoDirectory = this.videoDirectory,
            NewVideoDirectory = folder.Path.LocalPath
        };
        videoDirectory = folder.Path.AbsolutePath;
        await mediator.Publish(message);
    }

    [RelayCommand]
    private void Exit()
    {
        Environment.Exit(0);
    }
}
