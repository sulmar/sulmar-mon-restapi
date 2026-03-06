namespace OrderApi.Minimal.MessageHandlers;

public class JwtPropagationHandler(IHttpContextAccessor contextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = contextAccessor.HttpContext;

        var token = httpContext?.Request.Headers.Authorization.ToString();

        if (!string.IsNullOrEmpty(token))
            request.Headers.TryAddWithoutValidation("Authorization", token);

        return base.SendAsync(request, cancellationToken);
    }
}
