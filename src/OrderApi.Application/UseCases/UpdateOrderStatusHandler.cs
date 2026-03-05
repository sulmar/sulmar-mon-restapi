using OrderApi.Application.Contracts;
using OrderApi.Application.Exceptions;
using OrderApi.Domain;

namespace OrderApi.Application.UseCases;

public class UpdateOrderStatusHandler(IOrderRepository repository)
{
    public async Task HandleAsync(Guid id, OrderStatus targetStatus)
    {
        var order = await repository.GetByIdAsync(id);

        if (order is null)
            throw new OrderNotFoundException(id);

        order.TransitionTo(targetStatus);

        await repository.UpdateAsync(order);
    }
}
