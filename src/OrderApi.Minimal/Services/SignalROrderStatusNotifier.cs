using Microsoft.AspNetCore.SignalR;
using OrderApi.Application.Contracts;
using OrderApi.Domain;
using OrderApi.Minimal.Hubs;

namespace OrderApi.Minimal.Services;

public class SignalROrderStatusNotifier(IHubContext<OrdersHub> hub) : IOrderStatusNotifier
{
    public async Task NotifyStatusChangedAsync(Guid orderId, OrderStatus status, int version)
    {
        var dto = new OrderStatusChangedDto(orderId, status, version);

       // await hub.Clients.All.SendAsync("OrderStatusChanged", dto);

        await hub.Clients.Group($"order-{orderId}").SendAsync("OrderStatusChanged", dto);
    }
}

public record OrderStatusChangedDto(Guid OrderId, OrderStatus Status, int Version);