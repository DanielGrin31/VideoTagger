using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Services.Repositories
{
    public interface IVideoRepository
    {
        event EventHandler<VideoSourceUpdatedEventArgs> SourceUpdated;
        Task<string[]> GetAllVideosAsync();
        Task<string> GetVideoAsync(int index);
        Task SetVideosAsync(string[] videos);
        Task<int> GetCount();
    }
}