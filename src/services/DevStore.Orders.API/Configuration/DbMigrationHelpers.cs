using System;
using System.Linq;
using System.Threading.Tasks;
using DevStore.Orders.Domain.Vouchers;
using DevStore.Orders.Infra.Context;
using DevStore.WebAPI.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevStore.Orders.API.Configuration
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

            var ssoContext = scope.ServiceProvider.GetRequiredService<OrdersContext>();

            await DbHealthChecker.TestConnection(ssoContext);

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
                await ssoContext.Database.EnsureCreatedAsync();
                await EnsureSeedVouchers(ssoContext);
        }

        private static async Task EnsureSeedVouchers(OrdersContext context)
        {
            if (context.Vouchers.Any())
                return;

            await context.Vouchers.AddAsync(new Voucher("30-OFF",30,0,5000,VoucherDiscountType.Percentage, DateTime.Now.AddYears(5)));
            await context.Vouchers.AddAsync(new Voucher("50-OFF",0,50,5000, VoucherDiscountType.Value, DateTime.Now.AddYears(5)));

            await context.SaveChangesAsync();
        }

    }

}
