using System.Text.Json.Serialization;

namespace CatFactFetcher.Models;

public record CatFactResponse(
    [property: JsonPropertyName("fact")] string Fact,
    [property: JsonPropertyName("length")] int Length
);