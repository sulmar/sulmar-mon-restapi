using Microsoft.AspNetCore.Server.HttpSys;

namespace IdentityProvider.Api.Abstractions;

public interface IUserIdentityRepository
{
    Task<UserIdentity> GetAsync(string username);
}
