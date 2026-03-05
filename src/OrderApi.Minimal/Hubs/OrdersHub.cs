using Microsoft.AspNetCore.SignalR;

namespace OrderApi.Minimal.Hubs;

public class OrdersHub(ILogger<OrdersHub> logger) : Hub
{
    public override Task OnConnectedAsync()
    {
        // zla praktyka
        // logger.LogInformation($"Connected ConnectionId: {this.Context.ConnectionId}");

        logger.LogInformation("Connected ConnectionId: {ConnectionId}", Context.ConnectionId);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("Disconnected ConnectionId: {ConnectionId}", Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }


    public async Task Subscribe(Guid orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
    }
}
