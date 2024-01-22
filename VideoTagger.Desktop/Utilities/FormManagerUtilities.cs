using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using VideoTagger.Desktop.Models;

namespace VideoTagger.Desktop.Utilities;

public static class FormManagerUtilities
{
    public static bool TryParseGlobalFormConfig(string filePath, out GlobalFormConfig? config)
    {
        config = null;
        if (!File.Exists(filePath))
        {
            return false;
        }

        try
        {
            var globalConfigStr = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(globalConfigStr))
            {
                var defaultConfig = new FormConfig("default", []);
                config = new GlobalFormConfig(defaultConfig.FormName,
                    new Dictionary<string, FormConfig>() { { defaultConfig.FormName, defaultConfig } });
            }
            else
            {
                config = JsonSerializer.Deserialize<GlobalFormConfig>(globalConfigStr);
            }

            return true;
        }
        catch(Exception ex)
        {
            return false;
        }

    }
}