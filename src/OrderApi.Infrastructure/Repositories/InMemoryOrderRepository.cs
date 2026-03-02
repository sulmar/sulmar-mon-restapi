using OrderApi.Application.Contracts;
using OrderApi.Domain;
using System.Collections.Concurrent;

namespace OrderApi.Infrastructure.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();
    public Task AddAsync(Order order, CancellationToken ct = default)
    {
        if (!_orders.TryAdd(order.Id, order))
                throw new InvalidOperationException($"Order {order.Id} already exists");

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        _orders.TryRemove(id, out _);

        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
       Task.FromResult(_orders.TryGetValue(id, out var order) ? order : null);
    

    public Task<IReadOnlyList<Order>> GetListAsync(CancellationToken ct = default)
    {
       return Task.FromResult<IReadOnlyList<Order>>(_orders.Values.ToList());
    }

    public Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _orders[order.Id] = order;

        return Task.CompletedTask;
    }
}
