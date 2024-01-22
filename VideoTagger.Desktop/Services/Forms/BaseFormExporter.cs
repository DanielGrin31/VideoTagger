using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using VideoTagger.Desktop.Models;
using VideoTagger.Desktop.Utilities;

namespace VideoTagger.Desktop.Services.Forms;

public abstract class BaseFormExporter : IFormExporter
{
    public abstract string Extension { get; }
    public abstract Task ExportAsync(Dictionary<string, string> fields, string videoName, string formName);
    public abstract Task<Dictionary<string, Dictionary<string, string>>> ParseAsync(string formName);

    public void CreateNewVersion(string existing, FormConfig newConfig, bool fieldsChanged)
    {
        string existingFileName = $"{existing.Replace(' ', '_')}" + Extension;
        string existingFilePath = Path.Combine("Forms", existingFileName);
        if (!File.Exists(existingFilePath))
        {
            return;
        }
        if (fieldsChanged)
        {
            var backPath = existingFilePath.Replace(Extension,
                $".bak_{StringUtilities.GenerateRandomString(8)}_" + Extension);
            File.Move(existingFilePath, backPath);
        }
        else if (existing != newConfig.FormName)
        {
            string newFileName = $"{newConfig.FormName.Replace(' ', '_')}" + Extension;
            string newFilePath = Path.Combine("Forms", newFileName);
            File.Move(existingFilePath, newFilePath);
        }
    }

     void ZipFiles(string zipFilePath)
    {
        try
        {
            string sourceDirectory = "Forms";
            // Create a new zip file
            using ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create);
            // Get all files in the source directory
            string[] files = Directory.GetFiles(sourceDirectory);

            foreach (var file in files)
            {
                // Get the relative path of the file within the source directory
                string relativePath = Path.GetRelativePath(sourceDirectory, file);
                var extension = Path.GetExtension(relativePath);
                if (extension.ToLower() == this.Extension)
                {
                    // Create a new entry in the zip archive for each file
                    ZipArchiveEntry entry = archive.CreateEntry(relativePath);

                    // Open the file and copy its contents to the entry in the archive
                    using Stream entryStream = entry.Open();
                    using FileStream fileStream = File.OpenRead(file);
                    fileStream.CopyTo(entryStream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public Task BackupAllAsync()
    {
        ZipFiles("Forms/archive.zip");
        return Task.CompletedTask;
    }
}