namespace OrderApi.Application.Exceptions;

public class OrderNotFoundException : ApplicationException
{
    public Guid OrderId { get; }

    public OrderNotFoundException(Guid orderId)
        : base($"Order with id {orderId} was not found.")
    {
        OrderId = orderId;
    }
}
