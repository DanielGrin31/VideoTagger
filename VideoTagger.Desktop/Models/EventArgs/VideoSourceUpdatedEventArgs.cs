namespace VideoTagger.Desktop.Models.EventArgs
{
    public class VideoSourceUpdatedEventArgs(string[] videos) : System.EventArgs
    {
        public string[] Videos { get; } = videos;
    }
}