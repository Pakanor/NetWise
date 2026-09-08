namespace CatFactFetcher.Services;

public interface IFileWriterService
{
    Task AppendLineAsync(string filePath, string content, CancellationToken cancellationToken = default);
}