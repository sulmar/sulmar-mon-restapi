namespace OrderApi.Domain;

/*
 OrderItem
 ├── ProductId (Guid)
 ├── Quantity (int > 0)
 ├── UnitPrice (decimal >= 0)
 └── LineTotal (Quantity * UnitPrice)
*/

public class OrderItem
{
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}