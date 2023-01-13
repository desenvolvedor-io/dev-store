using DevStore.ShoppingCart.API.Services.gRPC;
using DevStore.WebAPI.Core.DatabaseFlavor;
using static DevStore.WebAPI.Core.DatabaseFlavor.ProviderConfiguration;
using DevStore.ShoppingCart.API.Data;
using DevStore.WebAPI.Core.Configuration;

namespace DevStore.ShoppingCart.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureProviderForContext<ShoppingCartContext>(DetectDatabase(configuration));

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

            services.AddDefaultHealthCheck(configuration);
        }

        public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Under certain scenarios, e.g minikube / linux environment / behind load balancer
            // https redirection could lead dev's to over complicated configuration for testing purpouses
            // In production is a good practice to keep it true
            if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
                app.UseHttpsRedirection();

            app.UseCors("Total");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGrpcService<ShoppingCartGrpcService>().RequireCors("Total");

            app.UseDefaultHealthcheck();
        }
    }
}