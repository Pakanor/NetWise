using CatFactFetcher.Services;
using FluentAssertions;
using Xunit;

namespace CatFactFetcher.Tests;

public class FileWriterServiceTests : IDisposable
{
    private readonly string _temporaryDirectory = Path.Combine(Path.GetTempPath(), $"CatFactFetcher.Tests-{Guid.NewGuid():N}");
    private readonly FileWriterService _fileWriterService = new();

    public FileWriterServiceTests()
    {
        Directory.CreateDirectory(_temporaryDirectory);
    }

    [Fact]
    public async Task AppendLineAsync_ShouldCreateFileWithContent()
    {
        var filePath = Path.Combine(_temporaryDirectory, "facts.txt");

        await _fileWriterService.AppendLineAsync(filePath, "First fact");

        File.Exists(filePath).Should().BeTrue();
        (await File.ReadAllTextAsync(filePath)).Should().Be($"First fact{Environment.NewLine}");
    }

    [Fact]
    public async Task AppendLineAsync_ShouldAppendContentToExistingFile()
    {
        var filePath = Path.Combine(_temporaryDirectory, "facts.txt");
        await File.WriteAllTextAsync(filePath, $"First fact{Environment.NewLine}");

        await _fileWriterService.AppendLineAsync(filePath, "Second fact");

        (await File.ReadAllTextAsync(filePath)).Should().Be(
            $"First fact{Environment.NewLine}Second fact{Environment.NewLine}");
    }

    public void Dispose()
    {
        if (Directory.Exists(_temporaryDirectory))
        {
            Directory.Delete(_temporaryDirectory, recursive: true);
        }
    }
}
