using OrderApi.Domain.Exceptions;

namespace OrderApi.Domain;

/*
Order
 ├── Id(Guid)
 ├── CustomerId(Guid)
 ├── CreatedAt(DateTimeOffset)
 ├── Status(OrderStatus)
 ├── Version(int)
 ├── Items(List<OrderItem>)
 └── TotalAmount(decimal – wyliczane)
*/

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; private set; }
    public int Version { get; set; }

    private List<OrderItem> _items = [];
    public IReadOnlyList<OrderItem> Items => _items;
    public decimal Total => Items.Sum(i => i.LineTotal);

    private Order(Guid id, Guid customerId, DateTime createdAt)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(nameof(id));

        if (customerId==Guid.Empty)
            throw new ArgumentException(nameof(customerId));


        Id = id;
        CustomerId = customerId;
        CreatedAt = createdAt;
        Status = OrderStatus.Draft;
        Version = 1;
    }

    // Metoda fabryczna (Fabric Method)
    public static Order Create(Guid id, Guid customerId)
    {
        return new Order(id, customerId, DateTime.Now);
    }

    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidStateTransitionException(Status, OrderStatus.Draft);

        _items.Add(new OrderItem
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice
        });

        IncrementVersion();
    }

    private void IncrementVersion()
    {
        Version++;
    }

    private void Place()
    {
        if (Status != OrderStatus.Draft)
        {
            throw new InvalidStateTransitionException(Status, OrderStatus.Placed);
        }

        Status = OrderStatus.Placed;
    }

    private void Pay()
    {
        if (Status != OrderStatus.Placed)
        {
            throw new InvalidOperationException();
        }

        Status = OrderStatus.Paid;

    }

    private void Cancel()
    {
        if (Status != OrderStatus.Draft &&  Status != OrderStatus.Placed)
        {
            throw new InvalidOperationException();
        }

        Status = OrderStatus.Cancelled;
    }

    public void TransitionTo(OrderStatus targetStatus)
    {
        switch(targetStatus)
        {
            case OrderStatus.Placed: Place(); break;
            case OrderStatus.Paid: Pay(); break;
            case OrderStatus.Cancelled: Cancel(); break;

            default: throw new InvalidStateTransitionException($"Uknown target status: {targetStatus}");
        }
    }


}
