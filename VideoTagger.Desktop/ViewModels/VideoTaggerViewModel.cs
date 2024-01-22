using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Models.EventArgs;
using VideoTagger.Desktop.Services;
using VideoTagger.Desktop.Services.Repositories;

namespace VideoTagger.Desktop.ViewModels
{
    public partial class VideoTaggerViewModel : ViewModelBase
    {
        private readonly IFormManager _forms;
        private readonly IVideoRepository _videoRepository;
        [ObservableProperty] private string[] _formNames;
        [ObservableProperty] private string _selectedFormName = "default";
        [ObservableProperty] ViewModelBase _videoForm;
        [ObservableProperty] private ViewModelBase _videoPlayer;
        [ObservableProperty] ObservableCollection<VideoReviewItem> _videoReviews = [];
        [ObservableProperty] private int _currentIndex = 0;
        private string _currentVideo = "";

        public string CurrentVideo
        {
            get { return _currentVideo; }
            set
            {
                SetProperty(ref _currentVideo, value);
                CurrentIndex = VideoReviews.ToList().FindIndex(x => x.VideoName == CurrentVideo);
            }
        }

        public VideoTaggerViewModel(
            IFormManager formManager, IVideoRepository videoRepository)
        {
            var player = this.Build<VideoPlayerViewModel>();
            var form = this.Build<VideoFormViewModel>();
            form.FormSubmitted += SubmitForm;
            VideoPlayer = player;
            VideoForm = form;
            _forms = formManager;
            _videoRepository = videoRepository;
            _videoRepository.SourceUpdated += VideoSourceUpdated;
            FormNames = _forms.GetFormNames();
        }

        private async void VideoSourceUpdated(object? sender, VideoSourceUpdatedEventArgs e)
        {
            await GenerateReviews(e.Videos);
            ((VideoPlayerViewModel)VideoPlayer).Videos = VideoReviews.Where(x => x.Status == ReviewStatus.NotSeen)
                .Select(x => x.VideoName).ToList();
            // generate videos list on the right
        }

        private async Task GenerateReviews(string[] videos)
        {
            var existing = await _forms.ParseAsync(SelectedFormName);
            var horrors = _videoRepository.GetHorrors();
            List<VideoReviewItem> reviews = new List<VideoReviewItem>();
            foreach (var video in videos)
            {
                string relativePath = Path.GetRelativePath(VideoLoader.CurrentFolder, video);
                if (horrors.Contains(video))
                {
                    reviews.Add(new VideoReviewItem(video, ReviewStatus.Horror));
                }
                else if (existing.ContainsKey(relativePath))
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
            var player = (VideoPlayer as VideoPlayerViewModel)!;
            string filePath = player.GetCurrentVideo();
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            string relativePath = Path.GetRelativePath(VideoLoader.CurrentFolder, filePath);
            await _forms.ExportAsync(e.Fields, relativePath,
                ((VideoFormViewModel)VideoForm).SelectedForm?.FormName ?? "default");
            var review = VideoReviews.First(x => x.VideoName == filePath);
            review!.Status = ReviewStatus.Seen;
            VideoReviews = new(VideoReviews.OrderBy(x => x.Status).ToList());
            player.RemoveVideo(filePath);
            player.PlayCurrentVideo();
        }

        [RelayCommand]
        private void VideoSelected(VideoSelectedEventArgs e)
        {
            var item = e.SelectedItem;
            var player = ((VideoPlayerViewModel)VideoPlayer);
            player.Videos = VideoReviews.Where(x => x.Status == ReviewStatus.NotSeen).Select(x => x.VideoName).ToList();
            player.SelectVideo(item.VideoName);
        }

        [RelayCommand]
        private void HorrorVideo()
        {
            var player = (VideoPlayer as VideoPlayerViewModel)!;
            string video = player.GetCurrentVideo();
            if (string.IsNullOrEmpty(video))
            {
                return;
            }

            var horror = VideoReviews.First(x => x.VideoName == video);
            horror.Status = ReviewStatus.Horror;
            VideoReviews = new(VideoReviews.OrderBy(x => x.Status).ToList());
            player.RemoveVideo(horror.VideoName);
            player.PlayCurrentVideo();
            _videoRepository.MarkHorror(video);
        }

        [RelayCommand]
        public async Task FormSelectionChanged(string formName)
        {
            var config = _forms.GetConfig(formName);

            var videos = await _videoRepository.GetAllVideosAsync();
            ((VideoFormViewModel)VideoForm).SelectedForm = config;
            await GenerateReviews(videos);
            ((VideoPlayerViewModel)VideoPlayer).Videos = VideoReviews.Where(x => x.Status == ReviewStatus.NotSeen)
                .Select(x => x.VideoName).ToList();

        }

        public void SetFormToDefault()
        {
            var formConfig = _forms.GetDefaultForm();
            SelectedFormName = formConfig!.FormName;

            ((VideoFormViewModel)VideoForm).SelectedForm = formConfig;
        }
    }
}