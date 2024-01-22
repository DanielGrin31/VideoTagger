using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Services;
using VideoTagger.Desktop.Utilities;

namespace VideoTagger.Desktop.ViewModels
{
    public partial class CreateFormViewModel : ViewModelBase
    {
        [ObservableProperty] private string _title = "Creating New Form";
        [ObservableProperty] private string _formName = "";
        private readonly IFormManager _formManager;
        public ObservableCollection<string> Forms { get; set; }
        private FormConfig? _selectedForm;

        public FormConfig? SelectedForm
        {
            get { return _selectedForm; }
            set
            {
                SetProperty(ref _selectedForm, value);
                NewFormCommand.NotifyCanExecuteChanged();
                SetDefaultCommand.NotifyCanExecuteChanged();
                DeleteFormCommand.NotifyCanExecuteChanged();
            }
        }

        [ObservableProperty] private ObservableCollection<FormField> _fieldsList;
        private bool FormIsSelected => SelectedForm is not null;

        public CreateFormViewModel(IFormManager formManager)
        {
            _formManager = formManager;
            NewForm();
            Forms = new(_formManager.GetFormNames());
        }

        [RelayCommand]
        private Task AddField()
        {
            FieldsList.Add(new("", FormFieldType.TextBox));
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task DeleteField(FormField field)
        {
            FieldsList.Remove(field);
            return Task.CompletedTask;
        }

        [RelayCommand]
        private void FormSelected(RoutedEventArgs e)
        {
            var ctrl = e.Source as Control;
            if (ctrl?.DataContext is string selected)
            {
                SelectedForm = _formManager.GetConfig(selected);

                FieldsList = new();
                for (int i = 0; i < SelectedForm.Fields.Length; i++)
                {
                    var field = SelectedForm.Fields[i];
                    FieldsList.Add(new(field.Name, field.FieldType) { Options = field.Options });
                }

                FormName = SelectedForm.FormName;

                Title = "Editing Form: " + FormName;
            }
        }

        [RelayCommand(CanExecute = nameof(FormIsSelected))]
        private void NewForm()
        {
            SelectedForm = null;
            FieldsList = new ObservableCollection<FormField>([new FormField("", FormFieldType.TextBox)]);
            FormName = "";
            Title = "Creating New Form";
        }

        [RelayCommand(CanExecute = nameof(FormIsSelected))]
        private void DeleteForm()
        {
            var deleted = _formManager.RemoveForm(SelectedForm.FormName);
            Forms.Remove(SelectedForm.FormName);
            SelectedForm = null;
        }

        [RelayCommand(CanExecute = nameof(FormIsSelected))]
        private void SetDefault()
        {
            _formManager.SetDefaultForm(SelectedForm.FormName);
        }

        [RelayCommand]
        private async Task SubmitForm()
        {
            var formConfig = new FormConfig(FormName, FieldsList.ToArray());
            if (SelectedForm is null)
            {
                bool exists = Forms.Any(x => x == formConfig.FormName);
                if (exists)
                {
                    await ErrorUtilities.ShowError("Form already exists with that name!");
                    return;
                }
                else
                {
                    _formManager.AddForm(formConfig);
                }
            }
            else
            {
                _formManager.EditForm(SelectedForm.FormName, formConfig);
                Forms.Remove(SelectedForm.FormName);
                SelectedForm = formConfig.Clone();
                
            }

            Forms.Add(FormName);
        }
    }
}