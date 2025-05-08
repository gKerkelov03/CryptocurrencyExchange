using Microsoft.EntityFrameworkCore;

using Application;
using Application.Domain;
using Application.Abstractions;

namespace Data;

public class DatabaseContext : DbContext, ITransientLifetime
{
    public DatabaseContext()
    {
        
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {

    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }

    public DbSet<Balance> Balances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name = Constants.AdminRoleName },
            new Role { Id = new Guid("22222222-2222-2222-2222-222222222222"), Name = Constants.UserRoleName }
        );

        modelBuilder.Entity<Cryptocurrency>().HasData(
            new Cryptocurrency { Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Bitcoin", CryptocurrencyId = "bitcoin" },
            new Cryptocurrency { Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Ethereum", CryptocurrencyId = "ethereum" },
            new Cryptocurrency { Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Tether", CryptocurrencyId = "tether" },
            new Cryptocurrency { Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "XRP", CryptocurrencyId = "ripple" },
            new Cryptocurrency { Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "BNB", CryptocurrencyId = "binancecoin" },
            new Cryptocurrency { Id = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "Solana", CryptocurrencyId = "solana" },
            new Cryptocurrency { Id = new Guid("11111111-2222-3333-4444-555555555555"), Name = "USD Coin", CryptocurrencyId = "usd-coin" },
            new Cryptocurrency { Id = new Guid("66666666-7777-8888-9999-000000000000"), Name = "Dogecoin", CryptocurrencyId = "dogecoin" },
            new Cryptocurrency { Id = new Guid("12345678-1234-1234-1234-123456789abc"), Name = "Cardano", CryptocurrencyId = "cardano" },
            new Cryptocurrency { Id = new Guid("abcdefab-cdef-cdef-cdef-abcdefabcdef"), Name = "TRON", CryptocurrencyId = "tron" },
            new Cryptocurrency { Id = new Guid("fedcbafe-dcba-dcba-dcba-fedcbafedcba"), Name = "Wrapped Bitcoin", CryptocurrencyId = "wrapped-bitcoin" },
            new Cryptocurrency { Id = new Guid("aabbccdd-eeff-0011-2233-445566778899"), Name = "Chainlink", CryptocurrencyId = "chainlink" },
            new Cryptocurrency { Id = new Guid("99887766-5544-3322-1100-aabbccddeeff"), Name = "Avalanche", CryptocurrencyId = "avalanche-2" },
            new Cryptocurrency { Id = new Guid("deadbeef-dead-beef-dead-deadbeefdead"), Name = "Stellar", CryptocurrencyId = "stellar" },
            new Cryptocurrency { Id = new Guid("beadfeed-bead-feed-bead-beadfeedbead"), Name = "Shiba Inu", CryptocurrencyId = "shiba-inu" }
        );
    }
}
