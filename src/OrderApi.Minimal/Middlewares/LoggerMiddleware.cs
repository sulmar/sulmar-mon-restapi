namespace OrderApi.Minimal.Middlewares;

public static class LoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseLogger(this IApplicationBuilder app) => app.UseMiddleware<LoggerMiddleware>();
}

public class LoggerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"#1 {context.Request.Method} {context.Request.Path}");

        await next(context);

        Console.WriteLine($"#1 {context.Response.StatusCode}");
    }
}
