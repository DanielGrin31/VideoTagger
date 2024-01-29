using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;
using static VideoTagger.Desktop.Utilities.FormManagerUtilities;

namespace VideoTagger.Desktop.Services.Forms
{
    public class FileFormManager : IFormManager
    {
        private const string FormConfigPath = "FormsConfig.txt";
        private GlobalFormConfig _config;
        private readonly IFormExporter _exporter;

        public FileFormManager(IFormExporter exporter)
        {
            _config = GetGlobalConfig();
            _exporter = exporter;
        }

        public bool AddForm(FormConfig form)
        {
            _config.Forms[form.FormName] = form;
            var serialized = JsonSerializer.Serialize(_config);
            File.WriteAllText(FormConfigPath, serialized);
            return true;
        }

        public Task ExportAsync(Dictionary<string, string> fields, string videoPath, string formName)
        {
            return _exporter.ExportAsync(fields, videoPath, formName);
        }

        public FormConfig? GetConfig(string formName)
        {
            if (!string.IsNullOrEmpty(formName) && _config.Forms.ContainsKey(formName))
            {
                return _config.Forms[formName].Clone();
            }

            return null;
        }

        public void SetGlobalConfig(GlobalFormConfig config)
        {
            _exporter.BackupAllAsync();
            _config = config;
            var serialized = JsonSerializer.Serialize(config);
            File.WriteAllText(FormConfigPath, serialized);
        }

        public FormConfig? GetDefaultForm()
        {
            var defaultFormName = _config.DefaultFormName;
            if (!string.IsNullOrEmpty(defaultFormName) && _config.Forms.ContainsKey(defaultFormName))
            {
                return _config.Forms[defaultFormName].Clone();
            }

            return _config.Forms["default"].Clone();
        }

        public string[] GetFormNames()
        {
            return _config.Forms.Select(x => x.Key).ToArray();
        }

        public GlobalFormConfig GetGlobalConfig()
        {
            if (!File.Exists(FormConfigPath))
            {
                File.Create(FormConfigPath).Close();
            }

            TryParseGlobalFormConfig(FormConfigPath, out GlobalFormConfig? globalConfig);
            return globalConfig!;
        }

        public Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName)
        {
            return _exporter.ParseAsync(formName);
        }

        public void EditForm(string existing, FormConfig newConfig)
        {
            var fieldsChanged = !newConfig.Fields.SequenceEqual(_config.Forms[existing].Fields);
            if (existing == newConfig.FormName)
            {
                // Same Name but fields may have changed
                _config.Forms[existing] = newConfig.Clone();
            }
            else
            {
                // Name Changed
                _config.Forms.Remove(existing);
                _config.Forms[newConfig.FormName] = newConfig;
                if (_config.DefaultFormName == existing)
                {
                    _config.DefaultFormName = newConfig.FormName;
                }
            }

            var serialized = JsonSerializer.Serialize(_config);
            File.WriteAllText(FormConfigPath, serialized);
            _exporter.CreateNewVersion(existing, newConfig, fieldsChanged);
        }

        public FormConfig? RemoveForm(string formName)
        {
            if (!_config.Forms.ContainsKey(formName))
            {
                return null;
            }

            var form = _config.Forms[formName];
            _config.Forms.Remove(formName);
            var serialized = JsonSerializer.Serialize(_config);
            File.WriteAllText(FormConfigPath, serialized);
            return form;
        }

        public FormConfig? SetDefaultForm(string formName)
        {
            if (_config.Forms.ContainsKey(formName))
            {
                _config.DefaultFormName = formName;
                var serialized = JsonSerializer.Serialize(_config);
                File.WriteAllText(FormConfigPath, serialized);
            }

            return null;
        }
    }
}