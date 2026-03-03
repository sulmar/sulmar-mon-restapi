using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Application.UseCases;
using OrderApi.Domain;
using System.Threading.Tasks;

namespace OrderApi.Application.UnitTests;

public class FakeOrderRepository : IOrderRepository
{
    private readonly IDictionary<Guid, Order> _orders = new Dictionary<Guid, Order>();
    public Task AddAsync(Order order, CancellationToken ct = default)
    {
        _orders.Add(order.Id, order);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return Task.FromResult<Order?>(_orders[id]);
    }

    public Task<IReadOnlyList<Order>> GetListAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidRequest_CreatesOrderAndAddsToRepository()
    {
        // Arrange
        var repository = new FakeOrderRepository();
        var handler = new CreateOrderHandler(repository);
        var customerId = Guid.NewGuid();
        var request = new CreateOrderRequest(customerId);

        // Act
        var order = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(OrderStatus.Draft, order.Status);
        Assert.Equal(1, order.Version);

        var orderFromRepo = await repository.GetByIdAsync(order.Id);
        Assert.NotNull(orderFromRepo);
        Assert.Equal(order.Id, orderFromRepo.Id);


    }
}
