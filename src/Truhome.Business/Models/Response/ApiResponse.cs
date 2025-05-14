using System.Text.Json.Serialization;

namespace Truhome.Business.Models.Response;

public record ApiResponse(
    [property: JsonPropertyName("isSuccess")] bool IsSuccess = true,
    [property: JsonPropertyName("result")] object? Result = null,
    [property: JsonPropertyName("error")] object? Error = null
);