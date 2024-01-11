using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using LibVLCSharp.Shared;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Services.Repositories;

namespace VideoTagger.Desktop.ViewModels
{
    public partial class VideoPlayerViewModel : ViewModelBase, IDisposable
    {

        public VideoPlayerViewModel()
        {
            _vlc = new();
            MediaPlayer = new MediaPlayer(_vlc);
        }
        public MediaPlayer MediaPlayer { get; }
        private int index = 0;
        private List<string> _videos = new List<string>();
        public List<string> Videos
        {
            get { return _videos; }
            set { _videos = value; index = 0; }
        }

        private LibVLC _vlc;
        private bool hasVideo = false;
        public string SelectIndex(int index)
        {
            if (!ValidIndex(index))
            {
                return "";
            }
            this.index = index;
            PlayCurrentVideo();
            return Videos[index];
        }
        public int SelectVideo(string videoName)
        {
            var index = Videos.IndexOf(videoName);
            if (index > 0)
            {
                this.index = index;
                PlayCurrentVideo();
                hasVideo = true;
            }
            else
            {
                if (File.Exists(videoName))
                {
                    using var media = new Media(_vlc, videoName);
                    MediaPlayer.Play(media);
                    hasVideo = true;
                    this.index = -1;
                }
            }
            return index;
        }
        [RelayCommand]
        public void PlayVideo()
        {
            if (hasVideo)
            {
                MediaPlayer.Pause();
            }
            else
            {
                PlayCurrentVideo();
                hasVideo = true;
            }
        }
        private bool ValidIndex(int index)
        {
            return index >= 0 && index < Videos.Count;
        }
        public string GetCurrentVideo()
        {
            if (!ValidIndex(index))
            {
                if (MediaPlayer.Media?.Mrl is not null)
                {
                    var uri = new Uri(MediaPlayer.Media.Mrl);
                    return uri.LocalPath;
                }
                return "";
            }
            return Videos[index];
        }
        [RelayCommand]
        public void PrevVideo()
        {
            if (index > 0)
            {
                index--;
            }
            PlayCurrentVideo();
            hasVideo = true;
        }

        [RelayCommand()]
        public void NextVideo()
        {

            if (index < Videos.Count - 1)
            {
                index++;
            }
            PlayCurrentVideo();
            hasVideo = true;
        }

        public void PlayCurrentVideo()
        {
            index=Math.Clamp(index,0,Videos.Count);
            var video = Videos[index];
            if (string.IsNullOrWhiteSpace(video))
            {
                return;
            }
            using var media = new Media(_vlc, video);
            MediaPlayer.Play(media);
        }
        public void Dispose()
        {
            _vlc?.Dispose();
            MediaPlayer?.Media?.Dispose();
            MediaPlayer?.Dispose();
        }

        internal void RemoveVideo(string filePath)
        {
            Videos.Remove(filePath);
        }
    }
}