using OrderApi.Domain;

namespace OrderApi.Application.DTOs;

public record UpdateOrderStatusRequest(OrderStatus Status);
