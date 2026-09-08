using System.Net;
using CatFactFetcher.Services;
using FluentAssertions;
using Moq;
using Moq.Contrib.HttpClient;
using NSubstitute;
using Xunit;

namespace CatFactFetcher.Tests;

public class CatFactServiceTests
{
    [Fact]
    public async Task GetRandomFactAsync_ShouldDeserializeJsonResponse()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler
            .SetupRequest(HttpMethod.Get, "https://catfact.ninja/fact")
            .ReturnsResponse(HttpStatusCode.OK, "{\"fact\":\"Cats sleep a lot.\",\"length\":20}", "application/json");

        using var client = handler.CreateClient();
        client.BaseAddress = new Uri("https://catfact.ninja/");
        var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<CatFactService>>();
        var service = new CatFactService(client, logger);

        var result = await service.GetRandomFactAsync();

        result.Should().NotBeNull();
        result!.Fact.Should().Be("Cats sleep a lot.");
        result.Length.Should().Be(20);
    }

    [Fact]
    public async Task GetRandomFactAsync_ShouldThrowWhenApiReturnsHttpError()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler
            .SetupRequest(HttpMethod.Get, "https://catfact.ninja/fact")
            .ReturnsResponse(HttpStatusCode.ServiceUnavailable);

        using var client = handler.CreateClient();
        client.BaseAddress = new Uri("https://catfact.ninja/");
        var logger = Substitute.For<Microsoft.Extensions.Logging.ILogger<CatFactService>>();
        var service = new CatFactService(client, logger);

        var action = () => service.GetRandomFactAsync();

        await action.Should().ThrowAsync<HttpRequestException>();
    }
}
