using System;
using System.Threading.Tasks;
using DevStore.Identity.API.Data;
using DevStore.WebAPI.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevStore.Identity.API.Configuration
{

    public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use command bellow:
        /// Nuget package manager: Add-Migration DbInit -context PagamentosContext
        /// Dotnet CLI: dotnet ef migrations add DbInit -c PagamentosContext
        /// </summary>
        public static async Task EnsureSeedData(IServiceScope serviceScope)
        {
            var services = serviceScope.ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            //var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var ssoContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await DbHealthChecker.TestConnection(ssoContext);

            if (env.IsDevelopment())
                await ssoContext.Database.EnsureCreatedAsync();
        }

    }

}
