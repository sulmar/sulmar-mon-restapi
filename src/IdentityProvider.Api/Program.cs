using IdentityProvider.Api.Abstractions;
using IdentityProvider.Api.Application;
using IdentityProvider.Api.Infrastructure;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserIdentityRepository, FakeUserIdentityRepository>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// HINT: Mozna podmienic implementacje na przyklad na BCrypt
// https://guptadeepak.com/comparative-analysis-of-password-hashing-algorithms-argon2-bcrypt-scrypt-and-pbkdf2/
builder.Services.AddScoped<IPasswordHasher<UserIdentity>, PasswordHasher<UserIdentity>>();

var app = builder.Build();

app.MapGet("/", () => "Hello Identity Api!");

// POST /login
// Content-Type: application/json
// { "username":"user1","password":"123" }
// 

app.MapPost("/login", async (LoginRequest request, IAuthService authService, ITokenService tokenService) =>
{
    var result = await authService.AuthorizeAsync(request.Username, request.Password);

    if (result.Success)
    {
        var accessToken = tokenService.GenerateAccessToken(result.Identity!);

        return Results.Ok(new { AccessToken = accessToken, RefeshToken = "xyz" });
    }

    return Results.Unauthorized();

});

app.Run();


record LoginRequest(string Username, string Password);