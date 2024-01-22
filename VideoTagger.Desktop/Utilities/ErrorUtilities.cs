using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace VideoTagger.Desktop.Utilities;

public class ErrorUtilities
{
    public static async Task ShowError(string error)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard("Error", error,
                ButtonEnum.Ok,Icon.Error);
        var result = await box.ShowAsync();

    }
}