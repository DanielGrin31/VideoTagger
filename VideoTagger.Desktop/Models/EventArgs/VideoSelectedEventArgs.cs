namespace VideoTagger.Desktop.Models.EventArgs;

public class VideoSelectedEventArgs(VideoReviewItem selectedItem) : System.EventArgs
{
    public VideoReviewItem SelectedItem { get; set; } = selectedItem;
}