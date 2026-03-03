using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Domain;
using OrderApi.Infrastructure.Repositories;
using OrderApi.Minimal.Endpoints;
using OrderApi.Minimal.ProblemDetails;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

var app = builder.Build();

app.ConfigureExceptionHandler();

var connectionString = app.Configuration["ConnectionStrings:OrderConnection"];

app.MapOrderApiEndpoints();

app.Run();
