using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace VideoTagger.Desktop.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [RelayCommand]
        public void NavigateTagger()
        {
            if (Parent is ShellViewModel shell)
            {
                shell.NavigateTo<VideoTaggerViewModel>(GenerateForm);
            }
        }

        private void GenerateForm(ViewModelBase vm)
        {
            if (vm is VideoTaggerViewModel videoTaggerVM)
            {
                videoTaggerVM.SetForm(new Dictionary<string, string>() { { "Name","text"},
                 { "Soldier", "check_Is Soldier?" },{"Place","combo_outdoor|indoor"} });
            }
        }
    }
}
