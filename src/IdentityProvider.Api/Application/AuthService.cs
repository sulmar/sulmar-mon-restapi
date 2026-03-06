using IdentityProvider.Api.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.Api.Application;

public class AuthService(IUserIdentityRepository repository,
    IPasswordHasher<UserIdentity> passwordHasher
    ) : IAuthService
{
    public async Task<AuthenticationResult> AuthorizeAsync(string username, string password)
    {
        var identity = await repository.GetAsync(username);

        var result =
            passwordHasher.VerifyHashedPassword(identity, identity.HashedPassword, password);

        if (username == identity.Username && result == PasswordVerificationResult.Success)
        {
            return new AuthenticationResult(true, identity);
        }

        return new AuthenticationResult(false);
    }
}
