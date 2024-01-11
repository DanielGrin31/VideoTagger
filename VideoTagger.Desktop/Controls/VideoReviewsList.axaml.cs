using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Controls;

public partial class VideoReviewsList : TemplatedControl
{
    public static readonly DirectProperty<VideoReviewsList, List<VideoReviewItem>> VideosProperty =
    AvaloniaProperty.RegisterDirect<VideoReviewsList, List<VideoReviewItem>>(
        nameof(Items),
        o => o.Items,
        (o, v) => o.Items = v);

    private List<VideoReviewItem> _items = new();

    public List<VideoReviewItem> Items
    {
        get { return _items; }
        set { SetAndRaise(VideosProperty, ref _items, value); }
    }
    public VideoReviewsList()
    {
        InitializeComponent();
    }
}