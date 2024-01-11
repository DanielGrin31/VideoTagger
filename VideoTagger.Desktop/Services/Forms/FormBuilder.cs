using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Services
{
    public class FormBuilder : IFormBuilder
    {
        public List<Control> BuildForm(IEnumerable<FormField> Fields,
         EventHandler<RoutedEventArgs> submitAction)
        {
            List<Control> controls = new List<Control>();
            foreach (var field in Fields)
            {
                Control control = null;
                switch (field.FieldType)
                {
                    case FormFieldType.TextBox:
                        control = BuildTextField(field);
                        break;
                    case FormFieldType.CheckBox:
                        control = BuildCheckField(field);
                        break;
                    case FormFieldType.ComboBox:
                        control = BuildComboField(field);
                        break;
                    default:
                        break;
                }
                if (control is not null)
                {
                    controls.Add(control);
                }
            }
            var button = new Button();
            button.Content = "Submit";
            controls.Add(button);
            button.Click += submitAction;
            return controls;
        }



        private Control BuildComboField(FormField field)
        {
            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            var fieldName = new TextBlock();
            fieldName.Text = field.Name;
            fieldName.VerticalAlignment = VerticalAlignment.Center;
            fieldName.Margin = new Thickness(0, 0, 5, 0);
            stack.Children.Add(fieldName);
            var comboBox = new ComboBox();
            comboBox.Name = field.Name;
            var itemsDesc = field.Options.Split(',');
            foreach (var item in itemsDesc)
            {
                var comboItem = new ComboBoxItem();
                comboItem.Content = item;
                comboBox.Items.Add(comboItem);
            }
            comboBox.SelectedIndex = 0;
            stack.Children.Add(comboBox);
            return stack;
        }

        private Control BuildCheckField(FormField field)
        {
            var checkBox = new CheckBox();
            checkBox.Name = field.Name;
            checkBox.Content =  field.Options;
            return checkBox;
        }

        private Control BuildTextField(FormField field)
        {
            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            var textBlock = new TextBlock();
            textBlock.Text = field.Name;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Margin = new Thickness(0, 0, 5, 0);
            var textBox = new TextBox();
            textBox.Name = field.Name;
            stack.Children.Add(textBlock);
            stack.Children.Add(textBox);
            return stack;
        }
    }
}