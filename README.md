# CatFactFetcher

Wymagania: .NET 8.0 SDK

## Uruchomienie

1. Klonowanie repozytorium:
   ```bash
   git clone https://github.com/Pakanor/NetWise.git
   cd NetWise

2. Uruchomienie aplikacji:
   dotnet run

## Architecture & Best Practices

- `BackgroundService` z `PeriodicTimer` do cyklicznego pobierania faktów.
- Polly Resilience Handler do obsługi chwilowych błędów HTTP.
- Walidacja opcji przez DataAnnotations.
- `LoggerMessage` Source Generators dla wydajnego logowania.
- Testy jednostkowe w xUnit.
- CI/CD przez GitHub Actions.