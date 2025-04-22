using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChangelogAutomation.Tests;

public sealed class TempFilePool : IDisposable
{
    private readonly List<string> _filePaths = [];

    public async Task<string> CreateTempFileAsync(Stream sourceStream)
    {
        var filePath = Path.GetTempFileName();

        try
        {
            await using var destinationStream = new FileStream(filePath, FileMode.Open);
            await sourceStream.CopyToAsync(destinationStream);
        }
        catch (Exception)
        {
            File.Delete(filePath);

            throw;
        }

        _filePaths.Add(filePath);

        return filePath;
    }

    public void Dispose()
    {
        foreach (var filePath in _filePaths)
            File.Delete(filePath);
    }
}
