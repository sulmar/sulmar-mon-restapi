using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Domain;
using OrderApi.Infrastructure.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

var app = builder.Build();

var connectionString = app.Configuration["ConnectionStrings:OrderConnection"];

app.MapGet("/", () => "Hello OrderApi!");

app.MapGet("/orders", async (IOrderRepository repository) => await repository.GetListAsync());
app.MapGet("/orders/{id:guid}", async (Guid id, IOrderRepository repository) =>
{
    var order = await repository.GetByIdAsync(id);

    if (order is null)
        return Results.NotFound();

    return Results.Ok(order);
});
app.MapPost("/orders", async (CreateOrderRequest request, IOrderRepository repository)=>
{
    if (request.CustomerId == Guid.Empty)
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["CustomerId"] = ["Customer ID is required"],
        });

    var order = Order.Create(Guid.NewGuid(), request.CustomerId);

    await repository.AddAsync(order);

    return Results.Created($"/orders/{order.Id}", order);
});

app.MapPatch("/orders/{id:guid}", async (Guid id, [FromBody] UpdateOrderStatusRequest request, IOrderRepository repository) =>
{
    var order = await repository.GetByIdAsync(id);

    if (order is null)
        return Results.NotFound();

    try
    {
        order.TransitionTo(request.Status);
    }
    catch(InvalidOperationException e)
    {
        return Results.Problem(
            detail: e.Message,
            statusCode: 409,
            title: "Invalid state transition",
            type: "http://localhost:5000/problems/invalid-state-transition"
            );
    }

    await repository.UpdateAsync(order);

    return Results.NoContent();

});

app.MapDelete("/orders/{id:guid}", async (Guid id, IOrderRepository repository) =>
{
    var order = await repository.GetByIdAsync(id);

    if (order is null)
        return Results.NotFound();

    await repository.DeleteAsync(id);

    return Results.NoContent();
});

app.Run();
