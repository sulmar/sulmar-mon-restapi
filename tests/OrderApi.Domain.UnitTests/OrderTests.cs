using OrderApi.Domain.Exceptions;

namespace OrderApi.Domain.UnitTests;

public class OrderTests
{
    // {Method}_{Scenario}_{ExpectedBehavior}

    [Fact]
    public void Create_ValidParameters_ReturnsOrderInDraftWithVersion1AndEmptyItemsAndCreatedAtIsNotMin()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        // Act
        var result = Order.Create(id, customerId);

        // Assert
        Assert.Equal(id, result.Id);
        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal(OrderStatus.Draft, result.Status);
        Assert.Equal(1, result.Version);
        Assert.Empty(result.Items);
        Assert.NotEqual(DateTime.MinValue, result.CreatedAt);
    }

    [Fact]
    public void Create_EmptyCustomerId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customerId = Guid.Empty;

        // Act
        Action act = () => Order.Create(id, customerId);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_EmptyOrderId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var customerId = Guid.NewGuid();

        // Act
        Action act = () => Order.Create(id, customerId);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Theory]
    [InlineData(3, 50, 3, 50, 150)]
    [InlineData(1, 50, 1, 50, 50)]
    public void AddItem_InDraft_AddsItemAndIncrementVersion(
        int quantity, 
        decimal unitPrice,
        int expectedQuantity, 
        decimal expectedUnitPrice, 
        decimal expectedLineTotal)
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act
        order.AddItem(productId, quantity, unitPrice);
        
        // Assert
        Assert.Equal(2, order.Version);
        Assert.NotEmpty(order.Items);

        var item = order.Items.Single();
        Assert.Equal(productId, item.ProductId);
        Assert.Equal(expectedQuantity, item.Quantity);
        Assert.Equal(expectedUnitPrice, item.UnitPrice);
        Assert.Equal(expectedLineTotal, item.LineTotal);
    }

    [Fact]
    public void AddItem_WhenNotDraft_ThrowsInvalidStateTransitionException()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid());
        var productId = Guid.NewGuid();
        order.TransitionTo(OrderStatus.Placed);

        // Act & Assert
        Assert.Throws<InvalidStateTransitionException>(() => order.AddItem(Guid.NewGuid(), quantity: 3, unitPrice: 50));
    }
   


}
