namespace CatFactFetcher.Services;

public class FileWriterService : IFileWriterService
{
    public async Task AppendLineAsync(string filePath, string content, CancellationToken cancellationToken = default)
    {
        await File.AppendAllTextAsync(filePath, $"{content}{Environment.NewLine}", cancellationToken);
    }
}