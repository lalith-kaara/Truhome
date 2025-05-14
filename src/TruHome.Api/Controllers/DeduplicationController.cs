using Microsoft.AspNetCore.Mvc;
using Truhome.Business.Models.Request;
using Truhome.Business.Models.Response;
using TruHome.Business.Models.Response;

namespace TruHome.Api.Controllers.V1
{
    [ApiVersion("2.0")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeduplicationController : ControllerBase
    {
        // [HttpGet]
        // [MapToApiVersion("1.0")]
        // public IActionResult GetV1() => Ok("This is v1");

        // [HttpGet]
        // [MapToApiVersion("2.0")]
        // public IActionResult GetV2() => Ok("This is v2");

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ApiResponse>> CheckDeduplicationV1([FromBody] DeduplicationRequest request, CancellationToken cancellationToken)
        {
            DeduplicationResponse response = new DeduplicationResponse
            {
                Matches = new List<Match>
                {
                    new Match
                    {
                        ExistingId = "1",
                        MatchType = "Exact",
                        Fields = new[] { "FirstName", "LastName", "MobileNumber" }
                    },
                    new Match
                    {
                        ExistingId = "2",
                        MatchType = "Ambiguous",
                        Fields = new[] { "FirstName", "MiddleName", "LastName", "FatherFirstName" }
                    }
                }
            };

            return new ApiResponse(Result: response);
        }
    }
}
