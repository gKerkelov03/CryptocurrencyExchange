using Microsoft.EntityFrameworkCore;

using Application;
using Application.Domain;

namespace Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role { Name = Constants.AdminRoleName },
            new Role { Name = Constants.UserRoleName }
        );


        modelBuilder.Entity<Cryptocurrency>().HasData(
            new Cryptocurrency { Name = "Bitcoin", CryptocurrencyId = "bitcoin" },
            new Cryptocurrency { Name = "Ethereum", CryptocurrencyId = "ethereum" },
            new Cryptocurrency { Name = "Tether", CryptocurrencyId = "tether" },
            new Cryptocurrency { Name = "XRP", CryptocurrencyId = "ripple" },
            new Cryptocurrency { Name = "BNB", CryptocurrencyId = "binancecoin" },
            new Cryptocurrency { Name = "Solana", CryptocurrencyId = "solana" },
            new Cryptocurrency { Name = "USD Coin", CryptocurrencyId = "usd-coin" },
            new Cryptocurrency { Name = "Dogecoin", CryptocurrencyId = "dogecoin" },
            new Cryptocurrency { Name = "Cardano", CryptocurrencyId = "cardano" },
            new Cryptocurrency { Name = "TRON", CryptocurrencyId = "tron" },
            new Cryptocurrency { Name = "Wrapped Bitcoin", CryptocurrencyId = "wrapped-bitcoin" },
            new Cryptocurrency { Name = "Chainlink", CryptocurrencyId = "chainlink" },
            new Cryptocurrency { Name = "Avalanche", CryptocurrencyId = "avalanche-2" },
            new Cryptocurrency { Name = "Stellar", CryptocurrencyId = "stellar" },
            new Cryptocurrency { Name = "Shiba Inu", CryptocurrencyId = "shiba-inu" }
        );
    }
}
