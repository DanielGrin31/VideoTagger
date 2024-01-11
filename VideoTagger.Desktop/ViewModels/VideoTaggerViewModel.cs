using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HarfBuzzSharp;
using LibVLCSharp.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class VideoTaggerViewModel : ViewModelBase
    {

        private IFormManager _forms;
        private readonly IVideoRepository videoRepository;
        [ObservableProperty]
        private string[] formNames;
        [ObservableProperty]
        private string _selectedFormName = "default";
        [ObservableProperty]
        ViewModelBase _videoForm;
        [ObservableProperty]
        ViewModelBase _videoPlayer;
        [ObservableProperty]
        ObservableCollection<VideoReviewItem> _videoReviews = new ObservableCollection<VideoReviewItem>();
        public VideoTaggerViewModel(
        IFormManager formManager, IVideoRepository videoRepository)
        {
            var player = this.Build<VideoPlayerViewModel>();

            var form = this.Build<VideoFormViewModel>();
            form.FormSubmitted += SubmitForm;
            VideoPlayer = player;
            VideoForm = form;
            _forms = formManager;
            this.videoRepository = videoRepository;
            this.videoRepository.SourceUpdated += VideoSourceUpdated;
            FormNames = _forms.GetFormNames();
        }

        private async void VideoSourceUpdated(object? sender, VideoSourceUpdatedEventArgs e)
        {
            await GenerateReviews(e.Videos);
            ((VideoPlayerViewModel)VideoPlayer).Videos = VideoReviews.Where(x => x.Status == ReviewStatus.NotSeen).Select(x => x.VideoName).ToList();
            // generate videos list on the right
        }

        private async Task GenerateReviews(string[] videos)
        {
            var existing = await _forms.ParseAsync(SelectedFormName);
            List<VideoReviewItem> reviews = new List<VideoReviewItem>();
            foreach (var video in videos)
            {
                string relativePath = Path.GetRelativePath(VideoLoader.CurrentFolder, video);
                if (existing.ContainsKey(relativePath))
                {
                    reviews.Add(new VideoReviewItem(video, ReviewStatus.Seen));
                }
                else
                {
                    reviews.Add(new VideoReviewItem(video, ReviewStatus.NotSeen));
                }
            }
            // parse existing form results
            VideoReviews = new ObservableCollection<VideoReviewItem>(reviews.OrderBy(x => x.Status));
        }
        private async void SubmitForm(object? sender, FormSubmittedEventArgs e)
        {
            var player = VideoPlayer as VideoPlayerViewModel;
            string filePath = player.GetCurrentVideo();
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            string relativePath = Path.GetRelativePath(VideoLoader.CurrentFolder, filePath);
            await _forms.ExportAsync(e.Fields, relativePath, ((VideoFormViewModel)VideoForm).SelectedForm?.FormName ?? "default");
            var review = VideoReviews.First(x => x.VideoName == filePath);
            review!.Status = ReviewStatus.Seen;
            VideoReviews = new(VideoReviews.OrderBy(x => x.Status).ToList());
            player.RemoveVideo(filePath);
            player.PlayCurrentVideo();
        }

        [RelayCommand]
        public void VideoSelected(TappedEventArgs e)
        {
            var item = (e.Source as Control).DataContext as VideoReviewItem;
            var player = ((VideoPlayerViewModel)VideoPlayer);
            player.Videos = VideoReviews.Where(x => x.Status == ReviewStatus.NotSeen).Select(x => x.VideoName).ToList();
            player.SelectVideo(item.VideoName);
        }

        [RelayCommand]
        public async Task HorrorVideo()
        {
            var player = (VideoPlayer as VideoPlayerViewModel);
            string video = player.GetCurrentVideo();
            var horror = VideoReviews.First(x => x.VideoName == video);
            horror.Status = ReviewStatus.Horror;
            VideoReviews = new(VideoReviews.OrderBy(x => x.Status).ToList());
            player.RemoveVideo(horror.VideoName);
            player.PlayCurrentVideo();
        }
        [RelayCommand]
        public async Task FormSelectionChanged(string formName)
        {
            var config = _forms.GetConfig(formName);
            ((VideoFormViewModel)VideoForm).SelectedForm = config;
            var videos = await videoRepository.GetAllVideosAsync();
            await GenerateReviews(videos);
        }
        public void SetFormToDefault()
        {
            var formConfig = _forms.GetDefaultForm();
            SelectedFormName = formConfig.FormName;

            ((VideoFormViewModel)VideoForm).SelectedForm = formConfig;
        }

    }
}
