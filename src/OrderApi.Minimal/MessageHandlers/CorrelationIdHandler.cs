
using NanoidDotNet;

namespace OrderApi.Minimal.MessageHandlers;

public class CorrelationIdHandler(IHttpContextAccessor contextAccessor) : DelegatingHandler
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = contextAccessor.HttpContext;

        var token = httpContext?.Request.Headers[CorrelationIdHeader].SingleOrDefault();

        if (string.IsNullOrEmpty(token))
            token = Nanoid.Generate(size: 5);

        request.Headers.TryAddWithoutValidation(CorrelationIdHeader, token);

        return base.SendAsync(request, cancellationToken);
    }
}
