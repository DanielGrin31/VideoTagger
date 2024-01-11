using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Services;

namespace VideoTagger.Desktop.ViewModels
{
    public partial class CreateFormViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string formName = "";
        private IFormManager _forms;

        public ObservableCollection<FormField> FieldsList { get; set; }
        public CreateFormViewModel(IFormManager formManager)
        {
            _forms = formManager;
            FieldsList = new ObservableCollection<FormField>(new List<FormField>()
            {
                new FormField("",FormFieldType.TextBox)
            });
        }

        [RelayCommand]
        public Task AddField()
        {
            FieldsList.Add(new("", FormFieldType.TextBox));
            return Task.CompletedTask;
        }
        [RelayCommand]
        public Task DeleteField(FormField field)
        {
            FieldsList.Remove(field);
            return Task.CompletedTask;
        }
        [RelayCommand]
        public Task SubmitForm()
        {
            var filename = FormName.Replace(' ', '_') + ".txt";
            var formConfig = new FormConfig(FormName, FieldsList);
            _forms.AddForm(formConfig);
            _forms.SetDefaultForm(FormName);
            return Task.CompletedTask;
        }
    }
}