using System.Diagnostics;

namespace OrderApi.Minimal.Middlewares;

public static class StopwatchMiddlewareExtensions
{
    public static IApplicationBuilder UseStopwatch(this IApplicationBuilder app)
    {
        app.UseMiddleware<StopwatchMiddleware>();

        return app;
    }
}

public class StopwatchMiddleware
{
    private readonly RequestDelegate next;

    public StopwatchMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var start = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            start.Stop();

            Console.WriteLine($"#2 {start.ElapsedMilliseconds} ms");

            context.Response.Headers.Add("X-Execution-Time", start.ElapsedMilliseconds.ToString());

            return Task.CompletedTask;
        });

        await next(context);
    }
}
