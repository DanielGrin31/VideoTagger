using CommunityToolkit.Mvvm.ComponentModel;

namespace VideoTagger.Desktop.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public ViewModelBase? Parent { get; set; }
}
