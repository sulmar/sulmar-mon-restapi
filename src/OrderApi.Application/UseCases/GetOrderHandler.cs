using OrderApi.Application.Contracts;
using OrderApi.Domain;

namespace OrderApi.Application.UseCases;

// Primary Constructor
public class GetOrderHandler(IOrderRepository _repository)
{
    public async Task<Order?> HandleAsync(Guid id) => await _repository.GetByIdAsync(id);
}