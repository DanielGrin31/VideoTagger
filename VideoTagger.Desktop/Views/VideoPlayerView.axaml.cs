using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using VideoTagger.Desktop.Services;
using VideoTagger.Desktop.ViewModels;

namespace VideoTagger.Desktop.Views
{
    public partial class VideoPlayerView : UserControl
    {
        public static readonly DirectProperty<VideoPlayerView, string> CurrentVideoProperty =
            AvaloniaProperty.RegisterDirect<VideoPlayerView, string>(
                nameof(CurrentVideo),
                o => o.CurrentVideo,
                (o, v) => o.CurrentVideo = v);

        private string _currentVideo;

        public string CurrentVideo
        {
            get { return _currentVideo; }
            set { SetAndRaise(CurrentVideoProperty, ref _currentVideo, value); }
        }
        public VideoPlayerView()
        {
            InitializeComponent();
        }

        
    }
}
