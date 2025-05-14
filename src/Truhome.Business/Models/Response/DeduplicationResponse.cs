using System.Text.Json.Serialization;

namespace Truhome.Business.Models.Response;

public class DeduplicationResponse
{
    [JsonPropertyName("matches")]
    public List<Match> Matches { get; init; } = new List<Match>();
}


public class Match
{
    [JsonPropertyName("existingId")]
    public string? ExistingId { get; init; }

    [JsonPropertyName("matchType")]
    public string? MatchType { get; init; }

    [JsonPropertyName("fields")]
    public string[]? Fields { get; init; }
}