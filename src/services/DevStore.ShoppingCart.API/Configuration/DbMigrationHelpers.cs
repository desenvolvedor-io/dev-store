using DevStore.ShoppingCart.API.Data;
using DevStore.WebAPI.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DevStore.ShoppingCart.API.Configuration
{
    public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use command bellow:
        /// Nuget package manager: Add-Migration DbInit -context OrdersContext
        /// Dotnet CLI: dotnet ef migrations add DbInit -c OrdersContext
        /// </summary>
        public static async Task EnsureSeedData(WebApplication app)
        {
            var services = app.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var ssoContext = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();

            await DbHealthChecker.TestConnection(ssoContext);

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
                await ssoContext.Database.EnsureCreatedAsync();

        }
    }
}
