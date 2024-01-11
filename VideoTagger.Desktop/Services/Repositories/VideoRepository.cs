using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Services.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private int currentIndex = 0;
        private string[] videos = new string[0];

        public event EventHandler<VideoSourceUpdatedEventArgs> SourceUpdated;

        public Task<string[]> GetAllVideosAsync()
        {
            return Task.FromResult(videos);
        }

        public Task<int> GetCount()
        {
            return Task.FromResult(videos.Length);
        }

        public Task<string> GetCurrentVideoAsync()
        {
            if (videos.Length == 0)
            {
                return Task.FromResult("");
            }
            return Task.FromResult(videos[currentIndex]);
        }

        public Task<string> GetVideoAsync(int index)
        {
            if (index >= videos.Length || index < 0)
            {
                return Task.FromResult("");
            }
            return Task.FromResult(videos[index]);
        }

        public Task<bool> HasNext()
        {
            return Task.FromResult(videos.Length - 1 > currentIndex);
        }


        public Task<int> MoveNextVideo()
        {
            if (currentIndex < videos.Length - 1)
            {
                currentIndex++;
            }
            return Task.FromResult(currentIndex);
        }

        public Task<int> MovePrevVideo()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
            }
            return Task.FromResult(currentIndex);
        }

        public Task SetVideosAsync(string[] videos)
        {
            this.videos = videos;
            currentIndex = 0;
            SourceUpdated?.Invoke(this, new VideoSourceUpdatedEventArgs(videos));
            return Task.CompletedTask;
        }
    }
}