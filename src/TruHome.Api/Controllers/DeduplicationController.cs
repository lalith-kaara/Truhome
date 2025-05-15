using Microsoft.AspNetCore.Mvc;
using Truhome.Api.Attributes;
using Truhome.Business.Interfaces;
using Truhome.Business.Models.Common;
using Truhome.Business.Models.Response;

namespace TruHome.Api.Controllers.V1
{
    [ApiVersion("2.0")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeduplicationController : ControllerBase
    {
        private readonly IDeduplicationManager _manager;
        public DeduplicationController(IDeduplicationManager manager)
        {
            _manager = manager;
        }

        // [HttpGet]
        // [MapToApiVersion("1.0")]
        // public IActionResult GetV1() => Ok("This is v1");

        // [HttpGet]
        // [MapToApiVersion("2.0")]
        // public IActionResult GetV2() => Ok("This is v2");

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
    }
}
