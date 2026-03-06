using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.Contracts;
using OrderApi.Application.DTOs;
using OrderApi.Application.UseCases;
using OrderApi.Domain;
using OrderApi.Infrastructure.Repositories;
using OrderApi.Infrastructure.Services;
using OrderApi.Minimal.Endpoints;
using OrderApi.Minimal.Hubs;
using OrderApi.Minimal.MessageHandlers;
using OrderApi.Minimal.Middlewares;
using OrderApi.Minimal.ProblemDetails;
using OrderApi.Minimal.Services;
using System.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();


builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<GetOrderHandler>();
builder.Services.AddScoped<UpdateOrderStatusHandler>();

builder.Services.AddServiceDiscovery();
builder.Services.AddHttpClient<ICurrencyRateService, NbpApiCurrencyRateService>(
    client => client.BaseAddress = new Uri("https://nbp-api"))
    .AddServiceDiscovery();


builder.Services.AddTransient<JwtPropagationHandler>();
builder.Services.AddTransient<CorrelationIdHandler>();
builder.Services.AddTransient<RequestTimingHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<IWarehouseService, WarehouseService>(client => client.BaseAddress = new Uri("https://warehouse-api"))
    .AddServiceDiscovery()
    .AddHttpMessageHandler<JwtPropagationHandler>()
    .AddHttpMessageHandler<CorrelationIdHandler>()
    .AddHttpMessageHandler<RequestTimingHandler>();

builder.Services.AddSignalR()
    .AddJsonProtocol(options => options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddScoped<IOrderStatusNotifier, SignalROrderStatusNotifier>();

var app = builder.Build();

app.UseStaticFiles();

app.ConfigureExceptionHandler();

var connectionString = app.Configuration["ConnectionStrings:OrderConnection"];

app.UseLogger();
app.UseStopwatch();

app.MapOrderApiEndpoints();

app.MapHub<OrdersHub>("/signalr/orders");



app.Run();

// HACK
public partial class Program { }
