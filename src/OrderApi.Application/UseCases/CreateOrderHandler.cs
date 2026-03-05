using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Domain;

namespace OrderApi.Application.UseCases;

public class CreateOrderHandler
{
    private readonly IOrderRepository _repository;
    private readonly ICurrencyRateService currencyRateService;
    private readonly IWarehouseService warehouseService;

    public CreateOrderHandler(IOrderRepository repository, ICurrencyRateService currencyRateService, IWarehouseService warehouseService )
    {
        _repository = repository;
        this.currencyRateService = currencyRateService;
        this.warehouseService = warehouseService;
    }

    public async Task<Order> HandleAsync(CreateOrderRequest request)
    {
        var order = Order.Create(Guid.NewGuid(), request.CustomerId);

        var rate = await currencyRateService.GetRate("EUR");

       await _repository.AddAsync(order);

        await warehouseService.Reserve(order.Id);

        return order;
    }
}
