using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.ViewModels;

namespace VideoTagger.Desktop.Views
{
    public partial class VideoFormView : UserControl
    {
        public VideoFormView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized()
        {
            var vm = ((VideoFormViewModel)DataContext);
            vm.FormConfigChanged += OnFormConfigChanged;

            base.OnInitialized();
        }

        private void OnFormConfigChanged(object? sender, FormConfigEventArgs e)
        {

            var vm = ((VideoFormViewModel)sender);
            var builder = vm?.Builder;
            if (builder != null)
            {
                var controls = builder.BuildForm(e.Fields, InvokeSubmitted);
                myForm.Children.Clear();
                myForm.Children.AddRange(controls);
            }

        }

        private Dictionary<string, string> GetStackPanelValues(StackPanel panel)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (var item in panel.Children)
            {
                switch (item)
                {
                    case TextBox textBox:
                        values.Add(textBox.Name!, textBox.Text!);
                        break;
                    case CheckBox checkBox:
                        values.Add(checkBox.Name!, checkBox.IsChecked.ToString()!);
                        break;
                    case ComboBox comboBox:
                        values.Add(comboBox.Name!, ((ComboBoxItem?)comboBox.SelectedItem)!.Content!.ToString()!);
                        break;
                    case StackPanel stack:
                        var dict = GetStackPanelValues(stack);
                        values = values.Concat(dict).ToDictionary();
                        break;
                }
            }
            return values;
        }
        private void InvokeSubmitted(object? sender, RoutedEventArgs e)
        {
            var fields = GetStackPanelValues(myForm);
            var vm = ((VideoFormViewModel)DataContext);
            vm.SubmitForm(fields);
        }
    }
}
