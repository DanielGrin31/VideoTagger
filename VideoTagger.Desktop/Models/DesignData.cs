using VideoTagger.Desktop.Services;
using VideoTagger.Desktop.Services.Forms;
using VideoTagger.Desktop.Services.Repositories;
using VideoTagger.Desktop.ViewModels;

namespace VideoTagger.Desktop.Models;

public static class DesignData
{
    public static VideoTaggerViewModel VideoTaggerVm => new(null, null);
    public static ShellViewModel ShellVm => new(null, null);
    public static VideoFormViewModel VideoFormVm => new(null);
    public static MainViewModel MainVm => new();
}