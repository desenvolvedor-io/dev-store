using DevStore.Billing.API.Configuration;
using DevStore.WebAPI.Core.Configuration;
using DevStore.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddMessageBusConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.RegisterServices();

var app = builder.Build();

await DbMigrationHelpers.EnsureSeedData(app);

app.UseApiCoreConfiguration(app.Environment);

app.Run();