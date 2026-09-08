using CatFactFetcher.Models;

namespace CatFactFetcher.Services;

public interface ICatFactService
{
    Task<CatFactResponse?> GetRandomFactAsync(CancellationToken cancellationToken = default);
}