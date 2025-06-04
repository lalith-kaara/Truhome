using System.Text.Json.Serialization;

namespace Truhome.Business.Models.Common
{
    public class Address
    {
        public int Id { get; set; }
        [JsonPropertyName("addressType")]
        public string? AddressType { get; set; }
        [JsonPropertyName("unitNo")]
        public string? UnitNo { get; set; }
        [JsonPropertyName("addLine1")]
        public string? AddLine1 { get; set; }
        [JsonPropertyName("addLine2")]
        public string? AddLine2 { get; set; }
        [JsonPropertyName("landmark")]
        public string? Landmark { get; set; }
        [JsonPropertyName("pinCode")]
        public string? PinCode { get; set; }
        [JsonPropertyName("areaOfLocality")]
        public string? AreaOfLocality { get; set; }
        [JsonPropertyName("city")]
        public string? City { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("customerid")]
        public int? Customerid { get; set; }
    }
}
