using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoTagger.Desktop.Services.Repositories
{
    public interface IVideoRepository
    {
        event EventHandler SourceUpdated;
        Task<string[]> GetAllVideosAsync();
        Task<string> GetVideoAsync(int index);
        Task<string> GetCurrentVideoAsync();
        Task<int> MoveNextVideo();
        Task<int> MovePrevVideo();
        Task<bool> HasNext();
        Task SetVideosAsync(string[] videos);
        Task<int> GetCount();
    }
}