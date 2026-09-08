using CatFactFetcher.Configuration;
using CatFactFetcher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddOptions<CatFactSettings>()
    .Bind(builder.Configuration.GetSection(CatFactSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<ICatFactService, CatFactService>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<CatFactSettings>>().Value;
    client.BaseAddress = new Uri(settings.ApiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddStandardResilienceHandler();

builder.Services.AddSingleton<IFileWriterService, FileWriterService>();
builder.Services.AddHostedService<CatFactBackgroundService>();

await builder.Build().RunAsync();