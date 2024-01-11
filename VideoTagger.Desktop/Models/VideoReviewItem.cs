using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VideoTagger.Desktop.Models
{
    public class VideoReviewItem:ObservableObject
    {
        private string videoName;
        public string VideoName
        {
            get { return videoName; }
            set { SetProperty(ref videoName,value); }
        }
        private ReviewStatus _status;
        public ReviewStatus Status
        {
            get { return _status; }
            set {  SetProperty(ref _status,value);  }
        }
               
        public VideoReviewItem(string name,ReviewStatus status)
        {
            this.VideoName=name;
            Status=status;
        }
    }
}