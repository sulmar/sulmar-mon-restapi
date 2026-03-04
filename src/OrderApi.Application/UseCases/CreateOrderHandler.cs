using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Domain;

namespace OrderApi.Application.UseCases;

public class CreateOrderHandler
{
    private readonly IOrderRepository _repository;
    private readonly ICurrencyRateService currencyRateService;

    public CreateOrderHandler(IOrderRepository repository, ICurrencyRateService currencyRateService)
    {
        _repository = repository;
        this.currencyRateService = currencyRateService;
    }

    public async Task<Order> HandleAsync(CreateOrderRequest request)
    {
        var order = Order.Create(Guid.NewGuid(), request.CustomerId);

        var rate = await currencyRateService.GetRate("EUR");

       await _repository.AddAsync(order);

        return order;
    }
}
