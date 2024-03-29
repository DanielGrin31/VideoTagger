using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Utilities;

namespace VideoTagger.Desktop.Services.Forms;

public class CsvFormExporter : BaseFormExporter
{
    private readonly IFileHasher _Hasher;
    public override string Extension { get; } = ".csv";

    public CsvFormExporter(IFileHasher hasher)
    {
        this._Hasher = hasher;
    }
    public override async Task ExportAsync(Dictionary<string, string> fields, string videoPath, string formName)
    {
        // Create a StringBuilder to store CSV content
        var existing = await ParseAsync(formName);
        var fileName = $"{formName.Replace(' ', '_')}" + Extension;
        var filePath = Path.Combine("Forms", fileName);
        string relativeVideoPath = Path.GetRelativePath(VideoLoader.CurrentFolder, videoPath);
        var hash = await _Hasher.GetHash(videoPath);
        string dataRow = $"{relativeVideoPath},{hash},{string.Join(",", fields.Values)}";
        StringBuilder csvContent = new StringBuilder();
        if (existing.TryGetValue(hash, out var value))
        {
            if (FieldValuesEqual(fields, value))
            {
                return;
            }

            var backPath = filePath.Replace(Extension,
                $".bak_{StringUtilities.GenerateRandomString(8)}_" + Extension);
            File.Move(filePath, backPath);
            using StreamReader sr = new StreamReader(backPath);
            await using StreamWriter sw = new StreamWriter(filePath);
            string? line = await sr.ReadLineAsync();
            while (line != null)
            {
                if (line.Split(',')[1] == hash)
                {
                    await sw.WriteLineAsync(dataRow);
                    line = await sr.ReadLineAsync();
                    continue;
                }

                await sw.WriteLineAsync(line);
                line = await sr.ReadLineAsync();
            }
            sr.Close();
            File.Delete(backPath);

        }
        else
        {
            if (existing.Count == 0)
            {
                // Add header row
                csvContent.AppendLine("Video Name,Hash," + string.Join(",", fields.Keys));
            }

            // Add data rows
            csvContent.AppendLine(dataRow);
            // Write content to the CSV file
            await File.AppendAllTextAsync(filePath, csvContent.ToString());
        }
    }

    private bool FieldValuesEqual(Dictionary<string, string> dictA, Dictionary<string, string> dictB)
    {
        if (dictA.Count != dictB.Count)
        {
            return false;
        }

        foreach ((string key, string value) in dictA)
        {
            if (!dictB.ContainsKey(key) || dictB[key] != (value ?? ""))
            {
                return false;
            }
        }

        return true;
    }

    public override async Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName)
    {
        Directory.CreateDirectory("Forms");
        string fileName = $"{formName.Replace(' ', '_')}" + Extension;
        string filePath = Path.Combine("Forms", fileName);
        await using FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
        fileStream.Close();
        var lines = await File.ReadAllLinesAsync(filePath);
        if (lines.Length == 0)
        {
            return new();
        }

        string[] columns = lines[0].Split(',');
        var finalDict = new Dictionary<string, Dictionary<string, string>>();
        foreach (var line in lines[1..])
        {
            Dictionary<string, string> rowDict = new Dictionary<string, string>();
            var values = line.Split(',');
            for (int i = 2; i < columns.Length; i++)
            {
                rowDict[columns[i]] = values[i];
            }

            finalDict[values[1]] = rowDict;
        }

        return finalDict;
    }
}