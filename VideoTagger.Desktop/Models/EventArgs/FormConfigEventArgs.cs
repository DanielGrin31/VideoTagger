using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTagger.Desktop.Models
{
    public class FormConfigEventArgs : EventArgs
    {
        public IEnumerable<FormField> Fields { get; set; }
    }
}
