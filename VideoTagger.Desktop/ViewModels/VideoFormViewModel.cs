using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Services;

namespace VideoTagger.Desktop.ViewModels
{
    public partial class VideoFormViewModel : ViewModelBase
    {
        public event EventHandler<FormConfigEventArgs>? FormConfigChanged;
        public event EventHandler<FormSubmittedEventArgs>? FormSubmitted;
        public IFormBuilder Builder { get; }
        private FormConfig? selectedForm;
    
        public FormConfig? SelectedForm
        {
            get => selectedForm;
            set => OnFormChange(ref selectedForm, value);
        }

        private void OnFormChange(ref FormConfig? selectedForm, FormConfig? value)
        {
            SetProperty(ref selectedForm, value);
            var fields = value.Fields;
            FormConfigChanged?.Invoke(this, new FormConfigEventArgs()
            {
                Fields = fields
            });
        }

        internal void SubmitForm(Dictionary<string, string> fields)
        {
            FormSubmitted?.Invoke(this, new FormSubmittedEventArgs(fields));
        }

        public VideoFormViewModel(IFormBuilder builder)
        {
            Builder = builder;
        }
    }
}