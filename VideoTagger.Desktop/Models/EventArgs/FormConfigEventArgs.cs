using System.Collections.Generic;

namespace VideoTagger.Desktop.Models.EventArgs
{
    public class FormConfigEventArgs(IEnumerable<FormField> fields) : System.EventArgs
    {
        public IEnumerable<FormField> Fields { get; set; } = fields;
    }
}
