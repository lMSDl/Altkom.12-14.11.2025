using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class LimiterFilter : IAsyncActionFilter
    {
        private int _limitPerMinute;
        public LimiterFilter(int limitPerMinute)
        {
            _limitPerMinute = limitPerMinute;
        }

        private int _counter;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_counter >= _limitPerMinute)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            Interlocked.Increment(ref _counter);
            try
            {
                await next();
            }
            finally
            {
                _ = Task.Delay(60000).ContinueWith(_ =>
                {
                    Interlocked.Decrement(ref _counter);
                });
            }


        }
    }
}
