using Microsoft.AspNetCore.Mvc;
using Truhome.Api.Attributes;
using Truhome.Business.Interfaces;
using Truhome.Business.Models.Common;
using Truhome.Business.Models.Response;

namespace Truhome.Api.Controllers
{
    [ApiVersion("2.0")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager _manager;
        public CustomerController(ICustomerManager manager)
        {
            _manager = manager;
        }
        
        [RequireOriginSystem]
        [AuthorizeKey]
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> CheckDeduplicationV1([FromBody] DeduplicationData request, CancellationToken cancellationToken)
        {
            Request.Headers.TryGetValue("x-correlation-id", out var correlationId);
            Request.Headers.TryGetValue("x-origin-system", out var originSystem);

            var result = await _manager.CheckDeduplicationAsync(request, correlationId, originSystem, cancellationToken).ConfigureAwait(false);

            return new ApiResponse(Result: result);
        }

        [RequireOriginSystem]
        [AuthorizeKey]
        [HttpPut]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> UpdateCustomerV1([FromBody] DeduplicationData request, CancellationToken cancellationToken)
        {
            Request.Headers.TryGetValue("x-correlation-id", out var correlationId);
            Request.Headers.TryGetValue("x-origin-system", out var originSystem);

            var result = await _manager.UpdateCustomerAsync(request, correlationId, originSystem, cancellationToken).ConfigureAwait(false);

            return new ApiResponse(Result: result);
        }
    }
}
