using CommunityToolkit.Mvvm.Input;
using VideoTagger.Desktop.Services;

namespace VideoTagger.Desktop.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
        }
        [RelayCommand]
        public void NavigateTagger()
        {
            if (Parent is ShellViewModel shell)
            {
                shell.NavigateTo<VideoTaggerViewModel>();
            }
        }
        [RelayCommand]
        public void NavigateForm()
        {
            if (Parent is ShellViewModel shell)
            {
                shell.NavigateTo<CreateFormViewModel>();
            }
        }

    }
}
