var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/products/{id:guid}/reservations", (Guid id, HttpContext context) =>
{
    var authHeader = context.Request.Headers.Authorization;

    return Results.Ok(new { Authorization = authHeader });
});

app.Run();

