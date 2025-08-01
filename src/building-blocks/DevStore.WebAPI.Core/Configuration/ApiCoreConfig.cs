using DevStore.WebAPI.Core.DatabaseFlavor;
using DevStore.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using static DevStore.WebAPI.Core.DatabaseFlavor.ProviderConfiguration;

namespace DevStore.WebAPI.Core.Configuration;

public static class ApiCoreConfig
{
    public static void AddLogger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog(new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger());
    }
    
    public static IServiceCollection AddApiCoreConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenApiDocument();

        services.AddDefaultHealthCheck(configuration);

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("Total",
                builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });

        return services;
    }

    public static IServiceCollection WithDbContext<TContext>(this IServiceCollection services,
        IConfiguration configuration) where TContext : DbContext
    {
        services.ConfigureProviderForContext<TContext>(DetectDatabase(configuration));

        return services;
    }

    public static void UseApiCoreConfiguration(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi();
            app.UseReDoc(options => { options.Path = "/redoc"; });
        }

        // Under certain scenarios, e.g. minikube / linux environment / behind load balancer
        // https redirection could lead devs to overcomplicate configuration for testing purposes
        // In production is a good practice to keep it true
        if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
            app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("Total");

        app.UseAuthConfiguration();

        app.MapControllers();

        app.UseDefaultHealthcheck();
    }
}