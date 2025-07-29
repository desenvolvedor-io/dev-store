using System;
using System.Threading.Tasks;
using DevStore.Billing.API.Data;
using DevStore.WebAPI.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevStore.Billing.API.Configuration;

public static class DbMigrationHelpers
{
    /// <summary>
    ///     Generate migrations before running this method, you can use command bellow:
    ///     Nuget package manager: Add-Migration DbInit -context BillingContext
    ///     Dotnet CLI: dotnet ef migrations add DbInit -c BillingContext
    /// </summary>
    public static async Task EnsureSeedData(WebApplication serviceScope)
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(services);
    }

    private static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var ssoContext = scope.ServiceProvider.GetRequiredService<BillingContext>();

        if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            await ssoContext.Database.EnsureCreatedAsync();

        await DbHealthChecker.TestConnection(ssoContext);
    }
}