using Microsoft.EntityFrameworkCore;

using Application;
using Application.Domain;
using Application.Abstractions;
using BCrypt.Net;

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

        var adminId = new Guid("33333333-3333-3333-3333-333333333333");
        var admin123Hashed = "$2a$11$5ZTVKnDSNDH4yokJ4P2SeeysSQJHqi0LqI3dtSafW1AcB42/iemS.";
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = adminId,
                Email = "admin@cryptoexchange.com",
                FirstName = "Admin",
                LastName = "User",
                Username = "admin",
                Password = admin123Hashed
            }
        );

        var userId = new Guid("44444444-4444-4444-4444-444444444444");
        var user123Hashed = "$2a$11$UzjXUrqJKkTEsvXftrkiUeexQnRNu3bpk7JWM8jCIU7II.TlfRg42";
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = userId,
                Email = "user@cryptoexchange.com",
                FirstName = "Regular",
                LastName = "User",
                Username = "user",
                Password = user123Hashed
            }
        );

        modelBuilder.Entity("RoleUser").HasData(
            new { RolesId = new Guid("11111111-1111-1111-1111-111111111111"), UsersId = adminId }, // Admin role for admin user
            new { RolesId = new Guid("22222222-2222-2222-2222-222222222222"), UsersId = userId }  // User role for regular user
        );

        modelBuilder.Entity<Balance>().HasData(
            new Balance
            {
                Id = new Guid("55555555-5555-5555-5555-555555555555"),
                Amount = 1.5, // 1.5 BTC
                UserId = adminId,
                CryptocurrencyId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") // Bitcoin
            },
            new Balance
            {
                Id = new Guid("66666666-6666-6666-6666-666666666666"),
                Amount = 10, // 10 ETH
                UserId = adminId,
                CryptocurrencyId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Ethereum
            }
        );

        modelBuilder.Entity<Balance>().HasData(
            new Balance
            {
                Id = new Guid("77777777-7777-7777-7777-777777777777"),
                Amount = 0.5, // 0.5 BTC
                UserId = userId,
                CryptocurrencyId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") // Bitcoin
            },
            new Balance
            {
                Id = new Guid("88888888-8888-8888-8888-888888888888"),
                Amount = 5, // 5 ETH
                UserId = userId,
                CryptocurrencyId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb") // Ethereum
            },
            new Balance
            {
                Id = new Guid("99999999-9999-9999-9999-999999999999"),
                Amount = 1000, // 1000 USDT
                UserId = userId,
                CryptocurrencyId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc") // Tether
            }
        );
    }
}
