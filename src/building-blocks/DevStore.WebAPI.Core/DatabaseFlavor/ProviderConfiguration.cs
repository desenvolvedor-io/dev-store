using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevStore.WebAPI.Core.DatabaseFlavor;

public class ProviderConfiguration
{
    private static readonly string MigrationAssembly =
        typeof(ProviderConfiguration).GetTypeInfo().Assembly.GetName().Name;

    private readonly string _connectionString;

    public ProviderConfiguration(string connString)
    {
        _connectionString = connString;
    }

    public Action<DbContextOptionsBuilder> SqlServer =>
        options => options.UseSqlServer(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));

    public Action<DbContextOptionsBuilder> MySql =>
        options => options.UseMySQL(_connectionString,  sql => sql.MigrationsAssembly(MigrationAssembly));

    public Action<DbContextOptionsBuilder> Postgre =>
        options =>
        {
            options.UseNpgsql(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        };

    public Action<DbContextOptionsBuilder> Sqlite =>
        options => options.UseSqlite(_connectionString, sql => sql.MigrationsAssembly(MigrationAssembly));

    public ProviderConfiguration With()
    {
        return this;
    }

    public static ProviderConfiguration Build(string connString)
    {
        return new ProviderConfiguration(connString);
    }


    /// <summary>
    ///     it's just a tuple. Returns 2 parameters.
    ///     Trying to improve readability at ConfigureServices
    /// </summary>
    public static (DatabaseType, string) DetectDatabase(IConfiguration configuration)
    {
        return (
            configuration.GetValue("AppSettings:DatabaseType", DatabaseType.None),
            configuration.GetConnectionString("DefaultConnection"));
    }
}