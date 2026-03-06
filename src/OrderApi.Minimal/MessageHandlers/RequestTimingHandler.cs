using System.Diagnostics;

namespace OrderApi.Minimal.MessageHandlers;

public class RequestTimingHandler(ILogger<RequestTimingHandler> logger) : DelegatingHandler
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await base.SendAsync(request, cancellationToken);
        stopwatch.Stop();

        var correlationId = request.Headers.GetValues(CorrelationIdHeader).FirstOrDefault();

        logger.LogInformation("[{CorrelationId}] {Method} {RequestUri} {Elapsed} ms", correlationId, request.Method, request.RequestUri, stopwatch.ElapsedMilliseconds);

        return response;
    }

}