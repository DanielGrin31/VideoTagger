using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace VideoTagger.Desktop.Services
{
    public class FormBuilder : IFormBuilder
    {
        public List<Control> BuildForm(Dictionary<string, string> Fields,
         EventHandler<RoutedEventArgs> submitAction)
        {
            List<Control> controls = new List<Control>();
            foreach ((string name, string desc) in Fields)
            {
                Control control = null;
                switch (desc.Split("_")[0])
                {
                    case "text":
                        control = BuildTextField(name, desc);
                        break;
                    case "check":
                        control = BuildCheckField(name, desc);
                        break;
                    case "combo":
                        control = BuildComboField(name, desc);
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



        private Control BuildComboField(string name, string desc)
        {
            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            var fieldName = new TextBlock();
            fieldName.Text = name;
            fieldName.VerticalAlignment = VerticalAlignment.Center;
            fieldName.Margin = new Thickness(0, 0, 5, 0);
            stack.Children.Add(fieldName);
            var comboBox = new ComboBox();
            comboBox.Name = name;
            var itemsDesc = desc.Split('_')[1].Split('|');
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

        private Control BuildCheckField(string name, string desc)
        {
            var checkBox = new CheckBox();
            checkBox.Name = name;
            checkBox.Content = string.Join(' ', desc.Split('_')[1..]);
            return checkBox;
        }

        private Control BuildTextField(string name, string desc)
        {
            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            var textBlock = new TextBlock();
            textBlock.Text = name;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Margin = new Thickness(0, 0, 5, 0);
            var textBox = new TextBox();
            textBox.Name = name;
            stack.Children.Add(textBlock);
            stack.Children.Add(textBox);
            return stack;
        }
    }
}