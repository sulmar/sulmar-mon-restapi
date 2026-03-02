namespace OrderApi.Domain;

public enum OrderStatus
{
    Draft,
    Placed,
    Paid,
    Shipped,
    Cancelled
}
