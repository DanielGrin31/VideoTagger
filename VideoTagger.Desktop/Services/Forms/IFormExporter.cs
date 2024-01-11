using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoTagger.Desktop.Services
{
    public interface IFormExporter
    {
        Task ExportAsync(Dictionary<string, string> Fields,string videoName,string formName);
        Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName);
    }
}