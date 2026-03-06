
namespace OrderApi.Minimal.BackgroundServices;

public class TimerWorker(ILogger<TimerWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Timer tick {Now}", DateTime.Now);

            await Task.Delay(Random.Shared.Next(1000, 5000));
        }
    }
}
