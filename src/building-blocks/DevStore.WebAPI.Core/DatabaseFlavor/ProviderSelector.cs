using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static DevStore.WebAPI.Core.DatabaseFlavor.ProviderConfiguration;

namespace DevStore.WebAPI.Core.DatabaseFlavor;

public static class ProviderSelector
{
    public static IServiceCollection ConfigureProviderForContext<TContext>(
        this IServiceCollection services,
        (DatabaseType, string) options) where TContext : DbContext
    {
        var (database, connString) = options;
        return database switch
        {
            DatabaseType.SqlServer => services.PersistStore<TContext>(Build(connString).With().SqlServer),
            DatabaseType.MySql => services.PersistStore<TContext>(Build(connString).With().MySql),
            DatabaseType.Postgre => services.PersistStore<TContext>(Build(connString).With().Postgre),
            DatabaseType.Sqlite => services.PersistStore<TContext>(Build(connString).With().Sqlite),

            _ => throw new ArgumentOutOfRangeException(nameof(database), database, null)
        };
    }

    public static Action<DbContextOptionsBuilder> WithProviderAutoSelection((DatabaseType, string) options)
    {
        var (database, connString) = options;
        return database switch
        {
            DatabaseType.SqlServer => Build(connString).With().SqlServer,
            DatabaseType.MySql => Build(connString).With().MySql,
            DatabaseType.Postgre => Build(connString).With().Postgre,
            DatabaseType.Sqlite => Build(connString).With().Sqlite,

            _ => throw new ArgumentOutOfRangeException(nameof(database), database, null)
        };
    }
}