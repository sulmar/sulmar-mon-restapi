using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Application.UseCases;
using OrderApi.Domain;
using OrderApi.Infrastructure.Repositories;
using OrderApi.Infrastructure.Services;
using OrderApi.Minimal.Endpoints;
using OrderApi.Minimal.Middlewares;
using OrderApi.Minimal.ProblemDetails;
using System.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<GetOrderHandler>();

builder.Services.AddServiceDiscovery();
builder.Services.AddHttpClient<ICurrencyRateService, NbpApiCurrencyRateService>(
    client => client.BaseAddress = new Uri("https://nbp-api"))
    .AddServiceDiscovery();

var app = builder.Build();

app.ConfigureExceptionHandler();

var connectionString = app.Configuration["ConnectionStrings:OrderConnection"];

app.UseLogger();
app.UseStopwatch();

app.MapOrderApiEndpoints();

app.Run();

// HACK
public partial class Program { }
