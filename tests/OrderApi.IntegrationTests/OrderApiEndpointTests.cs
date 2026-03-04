using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace OrderApi.IntegrationTests;

public class OrderApiEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    // dotnet add package Microsoft.AspNetCore.Mvc.Testing

    private readonly HttpClient _client;

    public OrderApiEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task SmokeTest_Returns200OK()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();


        Assert.Equal("Hello OrderApi!", body);

 

    }

    // TODO: Dodaj 1 test z GET   // hint: response.Content.ReadFromJsonAsync<T>
    // TODO: Dodaj 1 test z POST  // hint: client.PostAsJsonAsync<T>

}
