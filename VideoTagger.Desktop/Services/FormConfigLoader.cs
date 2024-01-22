using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoTagger.Desktop.Models;
using static VideoTagger.Desktop.Utilities.FormManagerUtilities;

namespace VideoTagger.Desktop.Services;

public class FormConfigLoader: INotificationHandler<FormConfigChangedMessage>
{
    private readonly IFormManager _formManager;

    public FormConfigLoader(IFormManager formManager)
    {
        _formManager = formManager;
    }
    public Task Handle(FormConfigChangedMessage notification, CancellationToken cancellationToken)
    {
        if (TryParseGlobalFormConfig(notification.ConfigFilePath, out GlobalFormConfig? config))
        {
            _formManager.SetGlobalConfig(config!);
        }

        return Task.CompletedTask;
    }
}