using DevStore.ShoppingCart.API.Data;
using DevStore.WebAPI.Core.Configuration;

namespace DevStore.ShoppingCart.API.Configuration
{
    public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use command bellow:
        /// Nuget package manager: Add-Migration DbInit -context ShoppingCartContext
        /// Dotnet CLI: dotnet ef migrations add DbInit -c ShoppingCartContext
        /// </summary>
        public static async Task EnsureSeedData(WebApplication app)
        {
            using var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ShoppingCartContext>();

            await DbHealthChecker.TestConnection(context);

            if (app.Environment.IsDevelopment())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }
    }
}
