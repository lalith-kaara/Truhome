
using Microsoft.AspNetCore.Mvc.Filters;
using Truhome.Business.Exceptions;

namespace Truhome.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AuthorizeKeyAttribute : ActionFilterAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
#if DEBUG
            return;
#endif
            IConfiguration configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = context.HttpContext.Request.Headers["X-Api-Key"].ToString();

            if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Equals(configuration["X-Api-Key"], StringComparison.OrdinalIgnoreCase) == false)
            {
                throw TruhomeExceptions.TE401;
            }
        }
        catch
        {
            throw TruhomeExceptions.TE401;
        }
    }

}
