using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Services.Forms
{
    public class FileFormManager : IFormManager
    {
        const string FormConfigPath = "FormsConfig.txt";
        private readonly GlobalFormConfig config;
        private IFormExporter _Exporter;

        public FileFormManager(IFormExporter exporter)
        {
            config = GetGlobalConfig();
            _Exporter=exporter;
        }

        public bool AddForm(FormConfig form)
        {
            config.Forms[form.FormName] = form;
            return true;
        }

        public Task ExportAsync(Dictionary<string, string> Fields, string videoName, string formName)
        {
            return _Exporter.ExportAsync(Fields,videoName,formName);
        }

        public FormConfig? GetConfig(string formName)
        {
            if (!string.IsNullOrEmpty(formName) && config.Forms.ContainsKey(formName))
            {
                return config.Forms[formName];
            }
            return null;
        }

        public FormConfig? GetDefaultForm()
        {
            var defaultFormName = config.DefaultFormName;
            if (!string.IsNullOrEmpty(defaultFormName))
            {
                return config.Forms[defaultFormName];
            }
            return null;
        }

        public string[] GetFormNames()
        {
            return config.Forms.Select(x=>x.Key).ToArray();
        }

        public GlobalFormConfig GetGlobalConfig()
        {
            if (!File.Exists(FormConfigPath))
            {
                File.Create(FormConfigPath);
            }
            var globalConfigStr = File.ReadAllText(FormConfigPath);
            if (string.IsNullOrEmpty(globalConfigStr))
            {
                var defaultConfig = new FormConfig("default", []);
                var global = new GlobalFormConfig(defaultConfig.FormName,
                new Dictionary<string, FormConfig>() { { defaultConfig.FormName, defaultConfig } });
                return global;
            }
            var globalConfig = JsonSerializer.Deserialize<GlobalFormConfig>(globalConfigStr);
            return globalConfig!;
        }

        public Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName)
        {
            return _Exporter.ParseAsync(formName);
        }

        public FormConfig? SetDefaultForm(string formName)
        {
            if (config.Forms.ContainsKey(formName))
            {
                config.DefaultFormName = formName;
                var serialized = JsonSerializer.Serialize(config);
                File.WriteAllText(FormConfigPath, serialized);
            }
            return null;
        }

 
    }
}