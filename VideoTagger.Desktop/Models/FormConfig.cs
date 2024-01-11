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
        public FormConfig(string FormName, IEnumerable<FormField> Fields)
        {
            this.FormName = FormName;
            this.Fields = new ObservableCollection<FormField>(Fields);
        }
        public string FormName { get; set; }
        public IEnumerable<FormField> Fields { get; set; }
    }

}