using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.UseCases;

public class CreateOrderHandler
{
    private readonly IOrderRepository _repository;

    public CreateOrderHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Order> HandleAsync(CreateOrderRequest request)
    {
        var order = Order.Create(Guid.NewGuid(), request.CustomerId);

       await _repository.AddAsync(order);

        return order;
    }
}
