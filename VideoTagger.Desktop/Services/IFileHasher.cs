using System.Threading.Tasks;

namespace VideoTagger.Desktop.Services;

public interface IFileHasher
{
    Task<string> GetHash(string filePath);
}