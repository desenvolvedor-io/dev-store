using DevStore.ShoppingCart.API.Data;
using DevStore.ShoppingCart.API.Services.gRPC;
using DevStore.WebAPI.Core.Configuration;

namespace DevStore.ShoppingCart.API.Configuration;

public static class ApiConfig
{
    public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiCoreConfiguration(configuration)
            .WithDbContext<ShoppingCartContext>(configuration)
            .AddGrpc();
    }

    public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
    {
        app.UseApiCoreConfiguration(env);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGrpcService<ShoppingCartGrpcService>().RequireCors("Total");
    }
}