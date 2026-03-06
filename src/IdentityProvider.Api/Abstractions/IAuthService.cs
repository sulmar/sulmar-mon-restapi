namespace IdentityProvider.Api.Abstractions;

public interface IAuthService
{
    Task<AuthenticationResult> AuthorizeAsync(string username, string password);
}

public record AuthenticationResult(bool Success, UserIdentity? Identity = null);

