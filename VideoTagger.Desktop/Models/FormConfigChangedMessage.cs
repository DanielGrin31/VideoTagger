using MediatR;

namespace VideoTagger.Desktop.Models;

public class FormConfigChangedMessage : INotification
{
    public string ConfigFilePath { get; set; }
}