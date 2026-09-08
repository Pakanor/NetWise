using CatFactFetcher.Configuration;
using CatFactFetcher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<CatFactSettings>(
            context.Configuration.GetSection(CatFactSettings.SectionName));

        services.AddHttpClient<ICatFactService, CatFactService>((serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<CatFactSettings>>().Value;
            client.BaseAddress = new Uri(settings.ApiBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(10);
        })
        .AddStandardResilienceHandler();

        services.AddSingleton<IFileWriterService, FileWriterService>();
    })
    .Build();

var catFactService = host.Services.GetRequiredService<ICatFactService>();
var fileWriterService = host.Services.GetRequiredService<IFileWriterService>();
var settings = host.Services.GetRequiredService<IOptions<CatFactSettings>>().Value;

var factResponse = await catFactService.GetRandomFactAsync();

if (factResponse is not null && !string.IsNullOrWhiteSpace(factResponse.Fact))
{
    await fileWriterService.AppendLineAsync(settings.OutputFilePath, factResponse.Fact);
}