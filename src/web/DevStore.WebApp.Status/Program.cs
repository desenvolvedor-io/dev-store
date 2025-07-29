using DevStore.WebAPI.Core.Configuration;
using DevStore.WebAPI.Core.DatabaseFlavor;
using DevStore.WebAPI.Core.Extensions;

using static DevStore.WebAPI.Core.DatabaseFlavor.ProviderConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);

var healthCheckBuilder = builder.Services.AddHealthChecksUI(setup =>
{
    setup.SetHeaderText("DevStore - Status Page");
    var endpoints = builder.Configuration.GetSection("ENDPOINTS").Get<string>();

    foreach (var endpoint in endpoints.Split(";"))
    {
        var name = endpoint.Split('|')[0];
        var uri = endpoint.Split('|')[1];

        setup.AddHealthCheckEndpoint(name, uri);
    }

    setup.UseApiEndpointHttpMessageHandler(sp => HttpExtensions.ConfigureClientHandler());
});
var (database, connString) = DetectDatabase(builder.Configuration);

switch (database)
{
    case DatabaseType.None:
        healthCheckBuilder.AddInMemoryStorage();
        break;
    case DatabaseType.SqlServer:
        healthCheckBuilder.AddSqlServerStorage(connString);
        break;
    case DatabaseType.MySql:
        healthCheckBuilder.AddMySqlStorage(connString);
        break;
    case DatabaseType.Postgre:
        healthCheckBuilder.AddPostgreSqlStorage(connString);
        break;
    case DatabaseType.Sqlite:
        healthCheckBuilder.AddSqliteStorage(connString);
        break;
    default:
        healthCheckBuilder.AddInMemoryStorage();
        break;
}

var app = builder.Build();

// Under certain scenarios, e.g minikube / linux environment / behind load balancer
// https redirection could lead dev's to over complicated configuration for testing purpouses
// In production is a good practice to keep it true
if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.MapHealthChecksUI(setup =>
{
    setup.AddCustomStylesheet("devstore.css");
    setup.UIPath = "/";
    setup.PageTitle = "DevStore - Status";
});
app.Run();