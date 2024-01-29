using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Services.Forms;
using VideoTagger.Desktop.Utilities;

namespace VideoTagger.Desktop.Services
{
    public class JsonFormExporter : BaseFormExporter
    {
        private readonly IFileHasher _Hasher;
        public override string Extension { get; } = ".txt";

        public JsonFormExporter(IFileHasher hasher)
        {
            _Hasher = hasher;
        }
        public override async Task ExportAsync(Dictionary<string, string> fields, string videoPath, string formName)
        {
            string fileName = $"{formName.Replace(' ', '_')}"+Extension;
            string relativeVideoPath = Path.GetRelativePath(VideoLoader.CurrentFolder, videoPath);

            var deserialized=await ParseAsync(formName);
            deserialized[relativeVideoPath] = fields;
            fields.Add("Hash",await _Hasher.GetHash(videoPath));
            string jsonString = JsonSerializer.Serialize(deserialized);
            File.WriteAllText(fileName, jsonString);
        }

        public override Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName)
        {
            Directory.CreateDirectory("Forms");
            string fileName = $"{formName.Replace(' ', '_')}"+Extension;
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