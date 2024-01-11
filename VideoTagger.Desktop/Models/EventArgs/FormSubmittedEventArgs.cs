using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VideoTagger.Desktop.Models
{
    public class FormSubmittedEventArgs:EventArgs
    {
        public Dictionary<string,string> Fields;

        public FormSubmittedEventArgs(Dictionary<string,string> fields)
        {
            Fields=fields;
        }

    }
}