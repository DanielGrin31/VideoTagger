using System;
using System.Collections.Generic;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Models.EventArgs;
using VideoTagger.Desktop.Services;

namespace VideoTagger.Desktop.ViewModels
{
    public  class VideoFormViewModel(IFormBuilder builder) : ViewModelBase
    {
        public event EventHandler<FormConfigEventArgs>? FormConfigChanged;
        public event EventHandler<FormSubmittedEventArgs>? FormSubmitted;
        public IFormBuilder Builder { get; } = builder;
        private FormConfig? _selectedForm;
    
        public FormConfig? SelectedForm
        {
            get => _selectedForm;
            set => OnFormChange(ref _selectedForm, value);
        }

        private void OnFormChange(ref FormConfig? selectedForm, FormConfig? value)
        {
            SetProperty(ref selectedForm, value);
            var fields = value?.Fields;
            FormConfigChanged?.Invoke(this, new FormConfigEventArgs(fields!));
        }

        internal void SubmitForm(Dictionary<string, string> fields)
        {
            FormSubmitted?.Invoke(this, new FormSubmittedEventArgs(fields));
        }
    }
}