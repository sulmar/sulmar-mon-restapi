namespace OrderApi.Application.Contracts;

public interface IWarehouseService
{
    Task Reserve(Guid productId);
}