using System.Text.Json.Serialization;
using Truhome.Business.Models.Common;

namespace Truhome.Business.Models.Response;

public class DeduplicationResponse
{
    [JsonPropertyName("matchCount")]
    public int MatchCount { get; init; }

    [JsonPropertyName("matches")]
    public List<Match>? Matches { get; init; } = new List<Match>();
}


public class Match
{
    [JsonPropertyName("existingId")]
    public string? ExistingId { get; init; }

    [JsonPropertyName("matchType")]
    public string? MatchType { get; init; }

    [JsonPropertyName("fields")]
    public DeduplicationData? Fields { get; init; }
}