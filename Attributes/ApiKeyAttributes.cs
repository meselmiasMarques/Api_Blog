using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttributes : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extractapi))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "Api Key não encontrada"
            };
            return;
        }
        if (!Configuration.ApiKey.Equals(extractapi))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 403,
                Content = "Acesso não autorizado",
            };
            return;
        }

        await next();
    }
}