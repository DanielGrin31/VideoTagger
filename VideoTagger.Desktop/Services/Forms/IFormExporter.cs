using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Services
{
    public interface IFormExporter
    {
        Task ExportAsync(Dictionary<string, string> Fields,string videoPath,string formName);
        Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName);
        void CreateNewVersion(string existing, FormConfig newConfig,bool fieldsChanged);
        Task BackupAllAsync();
    }
}