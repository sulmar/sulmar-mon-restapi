
using OrderApi.Application.Events;
using System.Threading.Channels;

namespace OrderApi.Minimal.BackgroundServices;

public class OrderProcessingWorker(
    ILogger<OrderProcessingWorker> logger,
    Channel<OrderPlacedEvent> channel
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // TODO: czekaj na Zmiane statusu zamowienia
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            // Pobieram zdarzenie z kanalu
            var eventData = await channel.Reader.ReadAsync(stoppingToken);

            var orderId = eventData.OrderId;

            logger.LogInformation("Processing {OrderId}", orderId);

            await Task.Delay(TimeSpan.FromSeconds(30)); // dlugo trwajaca operacja 

            logger.LogInformation("Order {OrderId} processing done.", orderId);
        }
    }
}
