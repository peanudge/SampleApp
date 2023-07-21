using System.Diagnostics;

namespace API.MiddleWrae;

public class ResponseTimeMiddleware
{
    private const string X_RESPONSE_TIME_MS = "X-Response-Time-ms";
    private readonly RequestDelegate _next;

    public ResponseTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var watch = new Stopwatch();
        watch.Start();
        context.Response.OnStarting(() =>
        {
            watch.Stop();
            var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
            context.Response.Headers[X_RESPONSE_TIME_MS] = responseTimeForCompleteRequest.ToString();
            return Task.CompletedTask;
        });
        return _next(context);
    }
}
