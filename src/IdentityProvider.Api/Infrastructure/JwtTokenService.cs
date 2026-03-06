using IdentityProvider.Api.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityProvider.Api.Infrastructure;


// dotnet Microsoft.IdentityModel.JsonWebTokens
public class JwtTokenService : ITokenService
{
    public string GenerateAccessToken(UserIdentity userIdentity)
    {
        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            [JwtRegisteredClaimNames.Name] = userIdentity.Username,
            [JwtRegisteredClaimNames.Email] = userIdentity.Email,
            [JwtRegisteredClaimNames.GivenName] = userIdentity.FirstName,
            [JwtRegisteredClaimNames.FamilyName] = userIdentity.LastName,
        };

        var secretKey = "a-string-secret-at-least-256-bits-long";

        var descriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            SigningCredentials = new SigningCredentials(
                                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                                                SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = "https://domain.com",
            Audience = "https://example.com"
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);

        return token;

    }
}
