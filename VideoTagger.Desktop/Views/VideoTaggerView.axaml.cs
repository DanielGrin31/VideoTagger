using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using VideoTagger.Desktop.Services;
using VideoTagger.Desktop.ViewModels;

namespace VideoTagger.Desktop.Views
{
    public partial class VideoTaggerView : UserControl
    {
        private readonly IFormBuilder builder;
        public VideoTaggerView(IFormBuilder builder)
        {
            this.builder = builder;
            InitializeComponent();
        }
        protected override void OnInitialized()
        {
            VideoTaggerViewModel? vm = (VideoTaggerViewModel?)DataContext;
            vm!.FormConfigChanged += Vm_FormConfigChanged;
            base.OnInitialized();
        }

        private void Vm_FormConfigChanged(object? sender, Models.FormConfigEventArgs e)
        {
            var controls = builder.BuildForm(e.Fields, OnFormSubmit);
            myForm.Children.AddRange(controls);
        }

        private void OnFormSubmit(object? sender, RoutedEventArgs e)
        {
            var values = GetStackPanelValues(myForm);
            var vm = (DataContext as VideoTaggerViewModel);
            vm.SubmitForm(values);
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


    }
}
