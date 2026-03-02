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
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}