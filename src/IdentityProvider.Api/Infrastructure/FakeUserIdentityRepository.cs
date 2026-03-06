using IdentityProvider.Api.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.Api.Infrastructure;

public class FakeUserIdentityRepository(IPasswordHasher<UserIdentity> passwordHasher) : IUserIdentityRepository
{
    public Task<UserIdentity> GetAsync(string username)
    {
        var identity = new UserIdentity
        {
            FirstName = "Johny",
            LastName = "Walker",
            Username = "johny",
            Email = "johny@domain.com",
        };

        identity.HashedPassword = passwordHasher.HashPassword(identity, "123");

        return Task.FromResult(identity);
    }
}
