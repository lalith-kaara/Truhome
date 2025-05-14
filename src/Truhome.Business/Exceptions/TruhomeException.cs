using System;
using System.Text.Json.Serialization;

namespace Truhome.Business.Exceptions;

public class TruhomeException : ApplicationException
{
    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; init; }

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; init; }

    public TruhomeException(string errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}
