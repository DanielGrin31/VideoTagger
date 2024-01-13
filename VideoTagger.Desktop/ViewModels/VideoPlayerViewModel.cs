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

    public string CurrentVideo
    {
        get
        {
            if (Videos.Count > 0&&ValidIndex(Index))
            {
                    return Videos[Index];
            }

            return "";
        }
    }

    private List<string> _videos = new();
    
    public List<string> Videos
    {
        get => _videos;
        set
        {
            SetProperty(ref _videos, value);
            Index = 0;
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
            PlayCurrentVideo();
            _hasVideo = true;
        }
        else
        {
            if (File.Exists(videoName))
            {
                using var media = new Media(_vlc, videoName);
                MediaPlayer.Play(media);
                _hasVideo = true;
                Index = -1;
            }
        }

        return videoIndex;
    }

    [RelayCommand]
    private void PlayVideo()
    {
        if (_hasVideo)
        {
            MediaPlayer.Pause();
        }
        else
        {
            PlayCurrentVideo();
            _hasVideo = true;
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
    private void PrevVideo()
    {
        if (Index > 0) Index--;

        PlayCurrentVideo();
        _hasVideo = true;
    }

    [RelayCommand()]
    private void NextVideo()
    {
        if (Index < Videos.Count - 1) Index++;

        PlayCurrentVideo();
        _hasVideo = true;
    }

    public void PlayCurrentVideo()
    {
        if (Videos.Count == 0) return;

        Index = Math.Clamp(Index, 0, Videos.Count-1);
        var video = Videos[Index];
        if (string.IsNullOrWhiteSpace(video)) return;

        using var media = new Media(_vlc, video);
        MediaPlayer.Play(media);
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