namespace TruHome.Business.Models.Response;

public record ApiResponse(bool IsSuccess = true, object? Result = null, object? Error = null);