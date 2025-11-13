using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class ConsoleLogFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("After:" + context.HttpContext.Response.StatusCode);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Before:" + context.HttpContext.Request.Method);
        }
    }
}
