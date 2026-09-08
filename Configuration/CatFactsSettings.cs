namespace CatFactFetcher.Configuration;

public class CatFactSettings
{
    public const string SectionName = "CatFactSettings";

    public string ApiBaseUrl { get; set; } = string.Empty;
    public string OutputFilePath { get; set; } = string.Empty;
}