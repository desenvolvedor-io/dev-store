using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevStore.Infra.Core.DatabaseFlavor;

public class ProviderConfiguration(string connString)
{
    private static readonly string MigrationAssembly =
        typeof(ProviderConfiguration).GetTypeInfo().Assembly.GetName().Name!;

    public Action<DbContextOptionsBuilder> SqlServer =>
        options => options.UseSqlServer(connString, sql => sql.MigrationsAssembly(MigrationAssembly));

    public Action<DbContextOptionsBuilder> MySql =>
        options => options.UseMySQL(connString, sql => sql.MigrationsAssembly(MigrationAssembly));

    public Action<DbContextOptionsBuilder> Postgre =>
        options =>
        {
            options.UseNpgsql(connString, sql => sql.MigrationsAssembly(MigrationAssembly));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        };

    public Action<DbContextOptionsBuilder> Sqlite =>
        options => options.UseSqlite(connString, sql => sql.MigrationsAssembly(MigrationAssembly));

    public ProviderConfiguration With()
    {
        return this;
    }

    public static ProviderConfiguration Build(string connString)
    {
        return new ProviderConfiguration(connString);
    }

    /// <summary>
    ///     Improve readability at ConfigureServices
    /// </summary>
    public static (DatabaseType, string) DetectDatabase(IConfiguration configuration)
    {
        return (
            configuration.GetValue("AppSettings:DatabaseType", DatabaseType.None),
            configuration.GetConnectionString("DefaultConnection"))!;
    }
}