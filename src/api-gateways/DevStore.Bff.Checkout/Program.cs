using DevStore.Bff.Checkout.Configuration;
using DevStore.WebAPI.Core.Configuration;
using DevStore.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.RegisterServices();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

builder.Services.ConfigureGrpcServices(builder.Configuration);


var app = builder.Build();


app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.Run();