using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace VideoTagger.Desktop.Services;

public class MD5FileHasher : IFileHasher
{
    public Task<string> GetHash(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        var bHash = md5.ComputeHash(stream);
        var hash = BitConverter.ToString(bHash).Replace("-", "").ToLowerInvariant();
        return Task.FromResult(hash);
    }
}