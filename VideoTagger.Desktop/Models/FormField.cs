using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VideoTagger.Desktop.Models
{
    public class FormField:ObservableObject
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name,value); }
        }
        
        private FormFieldType fieldType;
        public FormFieldType FieldType
        {
            get { return fieldType; }
            set { SetProperty(ref fieldType, value); }
        }
        private string options;
        public string Options
        {
            get { return options; }
            set { SetProperty(ref options, value); }
        }
        
        public FormField(string name, FormFieldType fieldType)
        {
            this.Name = name;
            FieldType = fieldType;
        }

        public override bool Equals(object? obj)
        {
            if (obj is FormField other)
            {
                return Name == other.Name&&FieldType==other.FieldType;
            }
            return false;
        }

    }
}