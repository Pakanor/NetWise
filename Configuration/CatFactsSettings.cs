using System.ComponentModel.DataAnnotations;

namespace CatFactFetcher.Configuration;

public class CatFactSettings
{
    public const string SectionName = "CatFactSettings";

    [Required]
    [Url]
    public string ApiBaseUrl { get; set; } = string.Empty;

    [Required]
    public string OutputFilePath { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int FetchIntervalSeconds { get; set; } = 10;
}