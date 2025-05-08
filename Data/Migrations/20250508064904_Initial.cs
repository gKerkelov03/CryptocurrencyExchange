using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cryptocurrencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CryptocurrencyId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cryptocurrencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CryptocurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balances_Cryptocurrencies_CryptocurrencyId",
                        column: x => x.CryptocurrencyId,
                        principalTable: "Cryptocurrencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Balances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cryptocurrencies",
                columns: new[] { "Id", "CryptocurrencyId", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-555555555555"), "usd-coin", "USD Coin" },
                    { new Guid("12345678-1234-1234-1234-123456789abc"), "cardano", "Cardano" },
                    { new Guid("66666666-7777-8888-9999-000000000000"), "dogecoin", "Dogecoin" },
                    { new Guid("99887766-5544-3322-1100-aabbccddeeff"), "avalanche-2", "Avalanche" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "bitcoin", "Bitcoin" },
                    { new Guid("aabbccdd-eeff-0011-2233-445566778899"), "chainlink", "Chainlink" },
                    { new Guid("abcdefab-cdef-cdef-cdef-abcdefabcdef"), "tron", "TRON" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "ethereum", "Ethereum" },
                    { new Guid("beadfeed-bead-feed-bead-beadfeedbead"), "shiba-inu", "Shiba Inu" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "tether", "Tether" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "ripple", "XRP" },
                    { new Guid("deadbeef-dead-beef-dead-deadbeefdead"), "stellar", "Stellar" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "binancecoin", "BNB" },
                    { new Guid("fedcbafe-dcba-dcba-dcba-fedcbafedcba"), "wrapped-bitcoin", "Wrapped Bitcoin" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "solana", "Solana" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "admin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Balances_CryptocurrencyId",
                table: "Balances",
                column: "CryptocurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_UserId",
                table: "Balances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balances");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Cryptocurrencies");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
