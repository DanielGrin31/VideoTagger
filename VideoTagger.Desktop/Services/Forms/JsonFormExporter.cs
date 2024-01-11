using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace VideoTagger.Desktop.Services
{
    public class JsonFormExporter : IFormExporter
    {
        public async Task ExportAsync(Dictionary<string, string> Fields, string videoName, string formName)
        {
            string fileName = $"{formName.Replace(' ', '_')}.txt";
            var deserialized=await ParseAsync(formName);
            deserialized[videoName] = Fields;
            string jsonString = JsonSerializer.Serialize(deserialized);
            File.WriteAllText(fileName, jsonString);
        }

        public Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName)
        {
            string fileName = $"{formName.Replace(' ', '_')}.txt";
            using FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            string json = File.ReadAllText(fileName);
            Dictionary<string, Dictionary<string, string>>? deserialized;
            if (string.IsNullOrEmpty(json))
            {
                deserialized = new Dictionary<string, Dictionary<string, string>>();
            }
            else
            {
                deserialized = JsonSerializer
                .Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
            }
            return Task.FromResult(deserialized!);
        }
    }
}