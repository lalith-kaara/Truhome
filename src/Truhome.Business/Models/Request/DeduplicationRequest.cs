using System.Text.Json.Serialization;

namespace Truhome.Business.Models.Request;

public class DeduplicationRequest
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; init; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    [JsonPropertyName("dateOfBirth")]
    public string? DateOfBirth { get; init; }

    [JsonPropertyName("mobileNumber")]
    public string? MobileNumber { get; init; }

    [JsonPropertyName("panNumber")]
    public string? PanNumber { get; init; }

    [JsonPropertyName("aadharNumber")]
    public string? AadharNumber { get; init; }

    [JsonPropertyName("fatherFirstName")]
    public string? FatherFirstName { get; init; }

    [JsonPropertyName("fatherMiddleName")]
    public string? FatherMiddleName { get; init; }

    [JsonPropertyName("fatherLastName")]
    public string? FatherLastName { get; init; }

    [JsonPropertyName("husbandFirstName")]
    public string? HusbandFirstName { get; init; }

    [JsonPropertyName("husbandMiddleName")]
    public string? HusbandMiddleName { get; init; }

    [JsonPropertyName("husbandLastName")]
    public string? HusbandLastName { get; init; }
}
