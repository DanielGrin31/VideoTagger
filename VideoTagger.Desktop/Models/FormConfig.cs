using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VideoTagger.Desktop.Models
{
    public class FormConfig
    {
        public FormConfig(string FormName, FormField[] Fields)
        {
            this.FormName = FormName;
            this.Fields = Fields;
        }
        public string FormName { get; set; }
        public FormField[] Fields { get; set; }

        public FormConfig Clone()
        {
            var fields = new FormField[Fields.Length];
            for (int i = 0; i < this.Fields.Length; i++)
            {
                fields[i] = new FormField(Fields[i].Name,Fields[i].FieldType)
                {
                    Options = Fields[i].Options
                };
            }
            return new FormConfig(this.FormName, fields);
        }
    }

}