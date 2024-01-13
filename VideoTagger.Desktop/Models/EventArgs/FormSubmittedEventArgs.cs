using System.Collections.Generic;

namespace VideoTagger.Desktop.Models.EventArgs
{
    public class FormSubmittedEventArgs(Dictionary<string, string> fields) : System.EventArgs
    {
        public readonly Dictionary<string,string> Fields = fields;
    }
}