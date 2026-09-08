using System.Net.Http.Json;
using CatFactFetcher.Models;
using Microsoft.Extensions.Logging;

namespace CatFactFetcher.Services;

public class CatFactService : ICatFactService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatFactService> _logger;

    public CatFactService(HttpClient httpClient, ILogger<CatFactService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CatFactResponse?> GetRandomFactAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CatFactResponse>("fact", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Wystąpił błąd podczas pobierania faktu z API.");
            throw;
        }
    }
}