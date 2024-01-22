using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Input;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Models.EventArgs;

namespace VideoTagger.Desktop.Controls;

public partial class VideoReviewsList : UserControl
{
    public event EventHandler<VideoSelectedEventArgs>? VideoSelected;
    

    
    
    // ReSharper disable once MemberCanBePrivate.Global
    public static readonly DirectProperty<VideoReviewsList, IEnumerable<VideoReviewItem>> VideosProperty =
        AvaloniaProperty.RegisterDirect<VideoReviewsList, IEnumerable<VideoReviewItem>>(
            nameof(Videos),
            o => o.Videos,
            (o, v) => o.Videos = v);

    private IEnumerable<VideoReviewItem> _videos = new AvaloniaList<VideoReviewItem>();

    public IEnumerable<VideoReviewItem> Videos
    {
        get => _videos;
        set
        {
            SetAndRaise(VideosProperty, ref _videos, value);
            VideosList.ItemsSource = Videos;
        }
    }

    
    public static readonly DirectProperty<VideoReviewsList, string?> SelectedVideoProperty =
        AvaloniaProperty.RegisterDirect<VideoReviewsList, string?>(
            nameof(SelectedVideo),
            o => o.SelectedVideo,
            (o, v) => o.SelectedVideo = v);

    private string? _selectedVideo ;

    public string? SelectedVideo
    {
        get => _selectedVideo;
        set
        {
            SetAndRaise(SelectedVideoProperty, ref _selectedVideo, value);
            if (!string.IsNullOrEmpty(value))
            {
                VideosList.SelectedItem = Videos.FirstOrDefault(x => x.VideoName == value);
            }
        }
    }
    protected override void OnInitialized()
    {
        VideosList.ItemsSource = Videos;
        base.OnInitialized();
    }

    public VideoReviewsList()
    {
        InitializeComponent();
    }

    private void VideosList_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        var item=((Control)e.Source!).DataContext as VideoReviewItem;
        VideoSelected?.Invoke(sender,new VideoSelectedEventArgs(item!));
    }
}