namespace IdentityProvider.Api.Abstractions;

public interface ITokenService
{
    string GenerateAccessToken(UserIdentity userIdentity);
}

