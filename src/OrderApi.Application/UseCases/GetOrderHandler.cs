using OrderApi.Application.Contracts;
using OrderApi.Domain;

namespace OrderApi.Application.UseCases;

public class GetOrderHandler
{
    private readonly IOrderRepository _repository;
    public GetOrderHandler(IOrderRepository repository)
    {
        _repository = repository;
    }
    public async Task<Order> HandleAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id);

        return order;
    }
}