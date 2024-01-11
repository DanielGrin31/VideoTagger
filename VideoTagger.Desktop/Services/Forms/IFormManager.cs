using System.Collections.Generic;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Services
{
    public interface IFormManager
    {
        GlobalFormConfig GetGlobalConfig();
        FormConfig? GetDefaultForm();
        FormConfig? GetConfig(string formName);
        FormConfig? SetDefaultForm(string formName);
        bool AddForm(FormConfig form);
        string[] GetFormNames();
        Task ExportAsync(Dictionary<string, string> Fields, string videoName, string formName);
        Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName);

    }
}