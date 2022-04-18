using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevStore.WebAPI.Core.DatabaseFlavor;

/// <summary>
/// SqlServer configuration
/// </summary>
public static class ContextConfiguration
{

    /// <summary>
    /// ASP.NET Identity Context config
    /// </summary>
    public static IServiceCollection PersistStore<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> databaseConfig) where TContext : DbContext
    {
        // Add a DbContext to store Keys. SigningCredentials and DataProtectionKeys
        if (services.All(x => x.ServiceType != typeof(TContext)))
            services.AddDbContext<TContext>(databaseConfig);
        return services;
    }


}