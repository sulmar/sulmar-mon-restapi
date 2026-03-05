using OrderApi.Application.Contracts;

namespace OrderApi.Infrastructure.Services;

public class WarehouseService(HttpClient http) : IWarehouseService
{
    public async Task Reserve(Guid productId)
    {
        await http.PostAsync($"/products/{productId}/reservations", null);
    }
}
