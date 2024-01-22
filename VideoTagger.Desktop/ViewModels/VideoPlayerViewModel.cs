using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibVLCSharp.Shared;

namespace VideoTagger.Desktop.ViewModels;
public partial class VideoPlayerViewModel : ViewModelBase, IDisposable
{
    public VideoPlayerViewModel()
    {
        _vlc = new LibVLC();
        MediaPlayer = new MediaPlayer(_vlc);
    }
    
    public MediaPlayer MediaPlayer { get; }

    private int Index
    {
        get => _index;
        set
        {
            SetProperty(ref _index, value);
            OnPropertyChanged(nameof(CurrentVideo));
        }
    }

    private string _currentVideo;

    public string CurrentVideo
    {
        get { return _currentVideo; }
        set
        {
            _currentVideo = value; 
            OnPropertyChanged(nameof(CurrentVideo));
        }
    }
    // public string CurrentVideo
    // {
    //     get
    //     {
    //         if (Videos.Count > 0&&ValidIndex(Index))
    //         {
    //                 return Videos[Index];
    //         }
    //         return "";
    //     }
    // }

    private List<string> _videos = new();
    
    public List<string> Videos
    {
        get => _videos;
        set
        {
            SetProperty(ref _videos, value);
            Index = 0;
            _hasVideo = false;
        }
    }

    private readonly LibVLC _vlc;
    private bool _hasVideo;
    private int _index;

    public string SelectIndex(int index)
    {
        if (!ValidIndex(index)) return "";

        Index = index;
        PlayCurrentVideo();
        return Videos[index];
    }

    public int SelectVideo(string videoName)
    {
        var videoIndex = Videos.IndexOf(videoName);
        if (videoIndex >= 0)
        {
            Index = videoIndex;
            _hasVideo=PlayCurrentVideo();
        }
        else
        {
            if (File.Exists(videoName))
            {
                using var media = new Media(_vlc, videoName);
                MediaPlayer.Play(media);
                _hasVideo = true;
                CurrentVideo = videoName;
                Index = -1;
            }
        }

        return videoIndex;
    }
    [RelayCommand]
    private void MoveMedia(int diff)
    {
        if (!_hasVideo)
        {
            return;
        }
        var time=MediaPlayer.Time + diff*1000;
        time=long.Clamp(time, 0, MediaPlayer.Media!.Duration);
        if (MediaPlayer.State == VLCState.Ended&&diff<0)
        {
            MediaPlayer.Stop();
            MediaPlayer.Play();
        }
        MediaPlayer.SeekTo(TimeSpan.FromMilliseconds(time));

    }
    [RelayCommand]
    private void PlayVideo()
    {
        if (_hasVideo)
        {
            switch (MediaPlayer.Media.State)
            {
                case VLCState.Ended:
                case VLCState.Error:
                    MediaPlayer.Stop();
                    MediaPlayer.Play();
                    break;
                default:
                    MediaPlayer.Pause();
                    break;                
            }
        }
        else
        {
            _hasVideo=PlayCurrentVideo();

        }
    }

    private bool ValidIndex(int index)
    {
        return index >= 0 && index < Videos.Count;
    }

    public string GetCurrentVideo()
    {
        if (!ValidIndex(Index))
        {
            if (MediaPlayer.Media?.Mrl is not null)
            {
                var uri = new Uri(MediaPlayer.Media.Mrl);
                return uri.LocalPath;
            }

            return "";
        }

        return Videos[Index];
    }
    [RelayCommand]
    private void ToggleMute()
    {
        MediaPlayer.ToggleMute();
    }
    [RelayCommand]
    private void PrevVideo()
    {
        if (Index > 0) Index--;

        _hasVideo=PlayCurrentVideo();

    }

    [RelayCommand()]
    private void NextVideo()
    {
        if (Index < Videos.Count - 1) Index++;

        _hasVideo=PlayCurrentVideo();

    }

    public bool PlayCurrentVideo()
    {
        if (Videos.Count == 0) return false;
        
        Index = Math.Clamp(Index, 0, Videos.Count-1);
        var video = Videos[Index];
        if (string.IsNullOrWhiteSpace(video)) return false;
        CurrentVideo = video;
        using var media = new Media(_vlc, video);
        return MediaPlayer.Play(media);
    }

    public void Dispose()
    {
        _vlc.Dispose();
        MediaPlayer.Media?.Dispose();
        MediaPlayer.Dispose();
    }

    internal void RemoveVideo(string filePath)
    {
        Videos.Remove(filePath);
    }
}