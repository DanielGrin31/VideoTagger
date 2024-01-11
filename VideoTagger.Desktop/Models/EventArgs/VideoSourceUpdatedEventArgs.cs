using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoTagger.Desktop.Models
{
    public class VideoSourceUpdatedEventArgs:EventArgs
    {
        public string[] Videos { get; }
        public VideoSourceUpdatedEventArgs(string[] videos)
        {
            this.Videos = videos;
            
        }
    }
}