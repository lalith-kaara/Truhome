using Microsoft.AspNetCore.Mvc.Filters;
using Truhome.Business.Exceptions;

namespace Truhome.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class RequireOriginSystemAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var hasHeader = context.HttpContext.Request.Headers.TryGetValue("x-origin-system", out var originSystem);

        if (!hasHeader || string.IsNullOrWhiteSpace(originSystem))
        {
            throw TruhomeExceptions.TE400;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
