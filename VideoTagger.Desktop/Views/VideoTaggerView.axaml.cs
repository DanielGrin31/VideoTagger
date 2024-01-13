using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using VideoTagger.Desktop.Services;
using VideoTagger.Desktop.ViewModels;

namespace VideoTagger.Desktop.Views
{
    public partial class VideoTaggerView : UserControl
    {
        public VideoTaggerView()
        {
            InitializeComponent();
        }
        protected override void OnLoaded(RoutedEventArgs e)
        {
            VideoTaggerViewModel? vm = (VideoTaggerViewModel?)DataContext;
            vm.SetFormToDefault();
            base.OnLoaded(e);
        }

   
    }
}
