

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection SetupSqlServer(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(
                "Server=localhost,1433;Database=CryptocurrencyExchange;User Id=sa;Password=StrongPassword1!;TrustServerCertificate=True;",
                builder => builder.MigrationsAssembly(typeof(DatabaseContext).Assembly))
            );

        return services;
    }
}
