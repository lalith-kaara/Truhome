using Faker;
using Microsoft.AspNetCore.Mvc;
using Truhome.Api.Attributes;
using Truhome.Business.Models.Common;
using Truhome.Business.Models.Response;

namespace TruHome.Api.Controllers.V1
{
    [AuthorizeKey]
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
        public async Task<ActionResult<ApiResponse>> CheckDeduplicationV1([FromBody] DeduplicationData request, CancellationToken cancellationToken)
        {
            var matches = new List<Match>();
            for (int i = 0; i < 5; i++)
            {
                var match = new Match
                {
                    ExistingId = RandomNumber.Next(1, 100),
                    MatchType = PickRandomString("Exact", "Ambiguous"),
                    Fields = new DeduplicationData
                    {
                        FirstName = Name.First(),
                        MiddleName = Name.Middle(),
                        LastName = Name.Last(),
                        DateOfBirth = Identification.DateOfBirth(),
                        MobileNumber = RandomNumber.Next(1111111111, 9999999999),
                        PanNumber = $"{RandomString(5).ToUpper()}{Faker.RandomNumber.Next(1000, 9999):D4}{RandomString(1).ToUpper()}",
                        AadharNumber = RandomNumber.Next(111111111111, 999999999999),
                        FatherFirstName = Name.First(),
                        FatherMiddleName = Name.Middle(),
                        FatherLastName = Name.Last(),
                        HusbandFirstName = Name.First(),
                        HusbandMiddleName = Name.Middle(),
                        HusbandLastName = Name.Last(),
                    }
                };
                matches.Add(match);
            }
            var response = new DeduplicationResponse
            {
                Matches = matches
            };

            return new ApiResponse(Result: response);
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string PickRandomString(string string1, string string2)
        {
            Random random = new Random();

            int randomIndex = random.Next(2);

            if (randomIndex == 0)
            {
                return string1;
            }
            else
            {
                return string2;
            }
        }
    }
}
