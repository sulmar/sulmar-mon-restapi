using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Domain;

namespace OrderApi.Minimal.Endpoints;

public static class OrderApiEndpoints
{
    public static IEndpointRouteBuilder MapOrderApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => "Hello OrderApi!");

        var orders = app.MapGroup("/orders");

        orders.MapGet("", async (IOrderRepository repository) => await repository.GetListAsync());
        orders.MapGet("{id:guid}", async (Guid id, IOrderRepository repository) =>
        {
            var order = await repository.GetByIdAsync(id);

            if (order is null)
                return Results.NotFound();

            return Results.Ok(order);
        });
        orders.MapPost("", async (CreateOrderRequest request, IOrderRepository repository) =>
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

        orders.MapPatch("{id:guid}", async (Guid id, [FromBody] UpdateOrderStatusRequest request, IOrderRepository repository) =>
        {
            var order = await repository.GetByIdAsync(id);

            if (order is null)
                return Results.NotFound();

            order.TransitionTo(request.Status);

            await repository.UpdateAsync(order);

            return Results.NoContent();

        });

        orders.MapDelete("{id:guid}", async (Guid id, IOrderRepository repository) =>
        {
            var order = await repository.GetByIdAsync(id);

            if (order is null)
                return Results.NotFound();

            await repository.DeleteAsync(id);

            return Results.NoContent();
        });

        return app;
    }
}
