using Truhome.Business.Models.Common;
using Truhome.Business.Models.Response;

namespace Truhome.Business.Interfaces;

public interface ICustomerManager
{
    Task<DeduplicationResponse> CheckDeduplicationAsync(DeduplicationData data, string? correlationId, string? originSystem, CancellationToken cancellationToken);
    Task<bool> UpdateCustomerAsync(DeduplicationData request, string? correlationId, string? originSystem, CancellationToken cancellationToken);
}
