using System.Text.Json.Serialization;

namespace Truhome.Business.Models.Common;

public class DeduplicationData
{
    [JsonPropertyName("customerId")]
    public int CustomerId { get; init; }

    [JsonPropertyName("externalCustomerId")]
    public string? ExternalCustomerId { get; init; }

    [JsonPropertyName("customerType")]
    public string? CustomerType { get; init; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    [JsonPropertyName("middleName")]
    public string? MiddleName { get; init; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    [JsonPropertyName("motherMaidenName")]
    public string? MotherMaidenName { get; init; }

    [JsonPropertyName("dateOfBirth")]
    public DateOnly? DateOfBirth { get; init; }

    [JsonPropertyName("emailId")]
    public string? EmailId { get; init; }

    [JsonPropertyName("gender")]
    public string? Gender { get; init; }

    [JsonPropertyName("mobileNumber")]
    public long? MobileNumber { get; init; }
    [JsonPropertyName("alternateMobileNumber")]
    public long? AlternateMobileNumber { get; init; }

    [JsonPropertyName("drivingLicenseNumber")]
    public string? DrivingLicenseNumber { get; init; }

    [JsonPropertyName("passportNumber")]
    public string? PassportNumber { get; init; }
    [JsonPropertyName("panNumber")]
    public string? PanNumber { get; init; }

    [JsonPropertyName("aadharNumber")]
    public string? AadharNumber { get; init; }

    [JsonPropertyName("ckycNumber")]
    public string? CkycId { get; init; }

    [JsonPropertyName("voterId")]
    public string? VoterId { get; init; }

    [JsonPropertyName("fatherFirstName")]
    public string? FatherFirstName { get; init; }

    [JsonPropertyName("fatherMiddleName")]
    public string? FatherMiddleName { get; init; }

    [JsonPropertyName("fatherLastName")]
    public string? FatherLastName { get; init; }

    [JsonPropertyName("spouseFirstName")]
    public string? SpouseFirstName { get; init; }

    [JsonPropertyName("spouseMiddleName")]
    public string? SpouseMiddleName { get; init; }

    [JsonPropertyName("spouseLastName")]
    public string? SpouseLastName { get; init; }
    [JsonPropertyName("companyName")]
    public string? CompanyName { get; init; }

    [JsonPropertyName("cin")]
    public string? CIn { get; init; }
    [JsonPropertyName("sourcesystem")]
    public string? SourceSystem { get; init; }

    public List<Address> Addresses { get; init; }
}
