var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/products/{id:guid}/reservations", async (Guid id, HttpContext context) =>
{
    var authHeader = context.Request.Headers.Authorization;

    await Task.Delay(Random.Shared.Next(50, 400)); // symulacja opóŸnienia systemu magazynowego

    return Results.Ok(new { Authorization = authHeader });
});

app.Run();

