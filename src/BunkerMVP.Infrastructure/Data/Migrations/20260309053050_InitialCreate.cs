using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BunkerMVP.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductCode = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ContactEmail = table.Column<string>(type: "text", nullable: false),
                    ContactPhone = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vessels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IMONumber = table.Column<string>(type: "text", nullable: false),
                    VesselType = table.Column<string>(type: "text", nullable: false),
                    Flag = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vessels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BunkerRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VesselId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    ETA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BunkerRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BunkerRequests_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BunkerRequests_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BunkerRequests_Vessels_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Vessels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierQuotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BunkerRequestId = table.Column<int>(type: "integer", nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierQuotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierQuotes_BunkerRequests_BunkerRequestId",
                        column: x => x.BunkerRequestId,
                        principalTable: "BunkerRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierQuotes_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BunkerOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BunkerRequestId = table.Column<int>(type: "integer", nullable: false),
                    SupplierQuoteId = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BunkerOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BunkerOrders_BunkerRequests_BunkerRequestId",
                        column: x => x.BunkerRequestId,
                        principalTable: "BunkerRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BunkerOrders_SupplierQuotes_SupplierQuoteId",
                        column: x => x.SupplierQuoteId,
                        principalTable: "SupplierQuotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "CreatedAt", "FullName", "PasswordHash", "Username" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System Administrator", "$2a$11$aQqFSwkbtyVU0muddz2zdupUDHYJvFqafKC27g3yr.9EMhVZAAGjm", "admin" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Country", "CreatedAt", "Name", "Port" },
                values: new object[,]
                {
                    { 1, "Singapore", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Singapore Port", "Singapore" },
                    { 2, "Netherlands", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rotterdam Port", "Rotterdam" },
                    { 3, "UAE", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fujairah Port", "Fujairah" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "ProductCode", "Unit" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Very Low Sulphur Fuel Oil", "VLSFO", "VLSFO-380", "MT" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Marine Gas Oil", "MGO", "MGO-DMA", "MT" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "High Sulphur Fuel Oil", "HSFO", "HSFO-380", "MT" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "ContactEmail", "ContactPhone", "Country", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, "ops@oceanfuel.sg", "+6561234567", "Singapore", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "OceanFuel Pte Ltd" },
                    { 2, "ops@euromarine.nl", "+31101234567", "Netherlands", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EuroMarine Fuels BV" },
                    { 3, "ops@gulfbunkers.ae", "+97141234567", "UAE", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gulf Bunkers LLC" }
                });

            migrationBuilder.InsertData(
                table: "Vessels",
                columns: new[] { "Id", "CreatedAt", "Flag", "IMONumber", "Name", "VesselType" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Panama", "IMO9234567", "MV Pacific Star", "Bulk Carrier" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Marshall Islands", "IMO9345678", "MV Atlantic Crown", "Tanker" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Liberia", "IMO9456789", "MV Indian Breeze", "Container" }
                });

            migrationBuilder.InsertData(
                table: "BunkerRequests",
                columns: new[] { "Id", "CreatedAt", "ETA", "LocationId", "ProductId", "Quantity", "Status", "UpdatedAt", "VesselId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 500m, "Quoted", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 200m, "Open", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, 3, 750m, "Ordered", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, 300m, "Draft", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 }
                });

            migrationBuilder.InsertData(
                table: "SupplierQuotes",
                columns: new[] { "Id", "BunkerRequestId", "CreatedAt", "Currency", "Notes", "Price", "SupplierId", "ValidUntil" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "USD", "Price includes delivery", 620m, 1, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "USD", "Best price guaranteed", 615m, 2, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "USD", "Fujairah ex-wharf", 645m, 3, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "BunkerOrders",
                columns: new[] { "Id", "BunkerRequestId", "CreatedAt", "Currency", "Notes", "OrderDate", "Status", "SupplierQuoteId", "TotalAmount" },
                values: new object[] { 1, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "USD", "Order confirmed", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Confirmed", 3, 483750m });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Username",
                table: "AdminUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BunkerOrders_BunkerRequestId",
                table: "BunkerOrders",
                column: "BunkerRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BunkerOrders_SupplierQuoteId",
                table: "BunkerOrders",
                column: "SupplierQuoteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BunkerRequests_LocationId",
                table: "BunkerRequests",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BunkerRequests_ProductId",
                table: "BunkerRequests",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BunkerRequests_VesselId",
                table: "BunkerRequests",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierQuotes_BunkerRequestId",
                table: "SupplierQuotes",
                column: "BunkerRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierQuotes_SupplierId",
                table: "SupplierQuotes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Vessels_IMONumber",
                table: "Vessels",
                column: "IMONumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "BunkerOrders");

            migrationBuilder.DropTable(
                name: "SupplierQuotes");

            migrationBuilder.DropTable(
                name: "BunkerRequests");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Vessels");
        }
    }
}
