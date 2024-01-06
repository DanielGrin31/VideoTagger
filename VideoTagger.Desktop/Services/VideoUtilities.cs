using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VideoTagger.Desktop.Services
{
    public class VideoUtilities
    {
        public static bool IsVideoFile(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            string[] videoExtensions = { ".mp4", ".avi", ".mkv", ".mov", ".wmv" }; // Add more extensions if needed
            return Array.Exists(videoExtensions, e => e.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}