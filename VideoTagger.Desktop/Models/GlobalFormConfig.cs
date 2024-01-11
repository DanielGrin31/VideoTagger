using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoTagger.Desktop.Models
{
    public class GlobalFormConfig
    {
        public GlobalFormConfig()
        {
            
        }
        public GlobalFormConfig(string defaultName,Dictionary<string,FormConfig> forms)
        {
            this.DefaultFormName=defaultName;
            Forms=forms;
        }

        public string? DefaultFormName { get; set; }
        public Dictionary<string, FormConfig> Forms { get; set; }=new Dictionary<string, FormConfig>();
    }
}