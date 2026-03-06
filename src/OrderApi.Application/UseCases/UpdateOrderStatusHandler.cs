using OrderApi.Application.Contracts;
using OrderApi.Application.Events;
using OrderApi.Application.Exceptions;
using OrderApi.Domain;
using System.Threading.Channels;

namespace OrderApi.Application.UseCases;

public class UpdateOrderStatusHandler(
    IOrderRepository repository,
    IOrderStatusNotifier notifier,
    Channel<OrderPlacedEvent> channel
    )
{
    public async Task HandleAsync(Guid id, OrderStatus targetStatus)
    {
        var order = await repository.GetByIdAsync(id);

        if (order is null)
            throw new OrderNotFoundException(id);

        order.TransitionTo(targetStatus);

        await repository.UpdateAsync(order);

        await notifier.NotifyStatusChangedAsync(id, order.Status, order.Version);

        if (targetStatus == OrderStatus.Placed)
            await channel.Writer.WriteAsync(new OrderPlacedEvent(order.Id, order.Version));


    }
}
