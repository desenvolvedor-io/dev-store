using DevStore.ShoppingCart.API.Services.gRPC;
using Microsoft.EntityFrameworkCore;

namespace DevStore.ShoppingCart.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Data.ShoppingCartContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddGrpc();

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
        }

        public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("Total");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGrpcService<ShoppingCartGrpcService>().RequireCors("Total");
        }
    }
}