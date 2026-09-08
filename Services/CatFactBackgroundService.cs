using CatFactFetcher.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatFactFetcher.Services;

public partial class CatFactBackgroundService : BackgroundService
{
    private readonly ICatFactService _catFactService;
    private readonly IFileWriterService _fileWriterService;
    private readonly CatFactSettings _settings;
    private readonly ILogger<CatFactBackgroundService> _logger;

    public CatFactBackgroundService(
        ICatFactService catFactService,
        IFileWriterService fileWriterService,
        IOptions<CatFactSettings> options,
        ILogger<CatFactBackgroundService> logger)
    {
        _catFactService = catFactService;
        _fileWriterService = fileWriterService;
        _settings = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_settings.FetchIntervalSeconds));

        do
        {
            await FetchAndSaveFactAsync(stoppingToken);
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task FetchAndSaveFactAsync(CancellationToken cancellationToken)
    {
        try
        {
            var factResponse = await _catFactService.GetRandomFactAsync(cancellationToken);

            if (factResponse is not null && !string.IsNullOrWhiteSpace(factResponse.Fact))
            {
                await _fileWriterService.AppendLineAsync(
                    _settings.OutputFilePath,
                    factResponse.Fact,
                    cancellationToken);

                LogFactSaved(_logger);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            LogCycleError(_logger, exception);
        }
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Pobrano i zapisano fakt o kotach.")]
    private static partial void LogFactSaved(ILogger logger);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "Błąd cyklu pobierania faktu o kotach.")]
    private static partial void LogCycleError(ILogger logger, Exception exception);
}