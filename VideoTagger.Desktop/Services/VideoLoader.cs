using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Services.Repositories;

namespace VideoTagger.Desktop.Services
{
    public class VideoLoader : INotificationHandler<VideoDirectoryChangedMessage>
    {
        public static string CurrentFolder = "";
        private readonly IVideoRepository videoRepository;

        public VideoLoader(IVideoRepository videoRepository)
        {
            this.videoRepository = videoRepository;
        }
        public Task Handle(VideoDirectoryChangedMessage notification, CancellationToken cancellationToken)
        {
            List<string> videos = new List<string>();
            string dirPath = notification.NewVideoDirectory;
            CurrentFolder = notification.NewVideoDirectory;
            
            var files = Directory.GetFiles(dirPath);
            foreach (var filePath in files)
            {
                if (VideoUtilities.IsVideoFile(filePath))
                {
                    videos.Add(filePath);
                }
            }
            videoRepository.SetVideosAsync(videos.ToArray());
            return Task.CompletedTask;
        }


    }
}
