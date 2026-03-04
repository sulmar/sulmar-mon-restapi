using Microsoft.AspNetCore.Mvc.Testing;
using OrderApi.Application.DTOs;
using OrderApi.Domain;
using System.Net;
using System.Net.Http.Json;

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

    [Fact]
    public async Task GetOrderById_ValidParameters_Returns200OK()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var content = JsonContent.Create(new { customerId });
        var createResponse = await _client.PostAsync("/orders", content);
        var responseBody = await createResponse.Content.ReadFromJsonAsync<Order>();

        // Act
        var getResponse = await _client.GetAsync($"/orders/{responseBody.Id}");
        var getResponseBody = await getResponse.Content.ReadFromJsonAsync<Order>();
        var statusCode = getResponse.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Equal(responseBody.Id, getResponseBody.Id);
    }

    [Fact]
    public async Task CreateOrder2_ValidParameters_Returns204Created()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var request = new CreateOrderRequest(customerId);

        // Act
        var response = await _client.PostAsJsonAsync("/orders", request);
        var statusCode = response.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.Created, statusCode);
    }
}
