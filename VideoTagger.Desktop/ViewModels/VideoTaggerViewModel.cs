using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HarfBuzzSharp;
using LibVLCSharp.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Services;
using VideoTagger.Desktop.Services.Repositories;

namespace VideoTagger.Desktop.ViewModels
{
    public partial class VideoTaggerViewModel : ViewModelBase, IDisposable
    {
        public event EventHandler<FormConfigEventArgs>? FormConfigChanged;
        public MediaPlayer MediaPlayer { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextVideoCommand))]

        private IVideoRepository _videos;
        private LibVLC _vlc;
        private bool hasVideo = false;
        public VideoTaggerViewModel(IVideoRepository videoRepository)
        {
            _videos = videoRepository;
            _vlc = new();
            _videos.SourceUpdated += UpdatedSource;
            MediaPlayer = new MediaPlayer(_vlc);
        }

        private async void UpdatedSource(object? sender, EventArgs e)
        {
            hasVideo = false;
            await PlayVideo();
            MediaPlayer.Pause();
        }

        public void SetForm(Dictionary<string, string> fields)
        {
            FormConfigChanged?.Invoke(this, new FormConfigEventArgs() { Fields = fields });
        }
        [RelayCommand]
        public void NavigateMain()
        {
            if (Parent is ShellViewModel shell)
            {
                shell.NavigateTo<MainViewModel>();
            }
        }
        [RelayCommand]
        public async Task PrevVideo()
        {
            await _videos.MovePrevVideo();
            await PlayCurrentVideo();
            hasVideo = true;
        }

        [RelayCommand()]
        public async Task NextVideo()
        {
            await _videos.MoveNextVideo();
            await PlayCurrentVideo();
            hasVideo = true;
        }
        private async Task PlayCurrentVideo()
        {
            var video = await _videos.GetCurrentVideoAsync();
            if (string.IsNullOrWhiteSpace(video))
            {
                return;
            }
            using var media = new Media(_vlc, video);
            MediaPlayer.Play(media);
        }
        [RelayCommand]
        public async Task PlayVideo()
        {
            if (hasVideo)
            {
                MediaPlayer.Pause();
            }
            else
            {
                await PlayCurrentVideo();
                hasVideo = true;
            }
        }
        [RelayCommand]
        public void StopVideo()
        {
            MediaPlayer.Pause();
        }

        public void Dispose()
        {
            _vlc?.Dispose();
            MediaPlayer?.Media?.Dispose();
            MediaPlayer?.Dispose();
        }

        public void SubmitForm(Dictionary<string, string> values)
        {

        }


    }
}
