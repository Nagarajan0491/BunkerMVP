using BunkerMVP.Domain.Entities;
using BunkerMVP.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BunkerMVP.Infrastructure.Data;

public class BunkerDbContext : DbContext
{
    public BunkerDbContext(DbContextOptions<BunkerDbContext> options) : base(options) { }

    public DbSet<Vessel> Vessels => Set<Vessel>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<BunkerRequest> BunkerRequests => Set<BunkerRequest>();
    public DbSet<SupplierQuote> SupplierQuotes => Set<SupplierQuote>();
    public DbSet<BunkerOrder> BunkerOrders => Set<BunkerOrder>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enums as strings
        modelBuilder.Entity<Product>()
            .Property(p => p.Unit)
            .HasConversion<string>();

        modelBuilder.Entity<BunkerRequest>()
            .Property(r => r.Status)
            .HasConversion<string>();

        modelBuilder.Entity<BunkerOrder>()
            .Property(o => o.Status)
            .HasConversion<string>();

        // Decimal precision
        modelBuilder.Entity<BunkerRequest>()
            .Property(r => r.Quantity)
            .HasPrecision(18, 4);

        modelBuilder.Entity<SupplierQuote>()
            .Property(q => q.Price)
            .HasPrecision(18, 4);

        modelBuilder.Entity<BunkerOrder>()
            .Property(o => o.TotalAmount)
            .HasPrecision(18, 4);

        // Unique indexes
        modelBuilder.Entity<Vessel>()
            .HasIndex(v => v.IMONumber)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.ProductCode)
            .IsUnique();

        modelBuilder.Entity<AdminUser>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // BunkerRequest relationships
        modelBuilder.Entity<BunkerRequest>()
            .HasOne(r => r.Vessel)
            .WithMany()
            .HasForeignKey(r => r.VesselId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BunkerRequest>()
            .HasOne(r => r.Product)
            .WithMany()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BunkerRequest>()
            .HasOne(r => r.Location)
            .WithMany()
            .HasForeignKey(r => r.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // SupplierQuote relationships
        modelBuilder.Entity<SupplierQuote>()
            .HasOne(q => q.BunkerRequest)
            .WithMany(r => r.SupplierQuotes)
            .HasForeignKey(q => q.BunkerRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SupplierQuote>()
            .HasOne(q => q.Supplier)
            .WithMany()
            .HasForeignKey(q => q.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // BunkerOrder relationships
        modelBuilder.Entity<BunkerOrder>()
            .HasOne(o => o.BunkerRequest)
            .WithOne(r => r.BunkerOrder)
            .HasForeignKey<BunkerOrder>(o => o.BunkerRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BunkerOrder>()
            .HasOne(o => o.SupplierQuote)
            .WithOne(q => q.BunkerOrder)
            .HasForeignKey<BunkerOrder>(o => o.SupplierQuoteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Vessel>().HasData(
            new Vessel { Id = 1, Name = "MV Pacific Star", IMONumber = "IMO9234567", VesselType = "Bulk Carrier", Flag = "Panama", CreatedAt = now },
            new Vessel { Id = 2, Name = "MV Atlantic Crown", IMONumber = "IMO9345678", VesselType = "Tanker", Flag = "Marshall Islands", CreatedAt = now },
            new Vessel { Id = 3, Name = "MV Indian Breeze", IMONumber = "IMO9456789", VesselType = "Container", Flag = "Liberia", CreatedAt = now }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "VLSFO", ProductCode = "VLSFO-380", Description = "Very Low Sulphur Fuel Oil", Unit = ProductUnit.MT, CreatedAt = now },
            new Product { Id = 2, Name = "MGO", ProductCode = "MGO-DMA", Description = "Marine Gas Oil", Unit = ProductUnit.MT, CreatedAt = now },
            new Product { Id = 3, Name = "HSFO", ProductCode = "HSFO-380", Description = "High Sulphur Fuel Oil", Unit = ProductUnit.MT, CreatedAt = now }
        );

        modelBuilder.Entity<Location>().HasData(
            new Location { Id = 1, Name = "Singapore Port", Port = "Singapore", Country = "Singapore", CreatedAt = now },
            new Location { Id = 2, Name = "Rotterdam Port", Port = "Rotterdam", Country = "Netherlands", CreatedAt = now },
            new Location { Id = 3, Name = "Fujairah Port", Port = "Fujairah", Country = "UAE", CreatedAt = now }
        );

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier { Id = 1, Name = "OceanFuel Pte Ltd", ContactEmail = "ops@oceanfuel.sg", ContactPhone = "+6561234567", Country = "Singapore", CreatedAt = now },
            new Supplier { Id = 2, Name = "EuroMarine Fuels BV", ContactEmail = "ops@euromarine.nl", ContactPhone = "+31101234567", Country = "Netherlands", CreatedAt = now },
            new Supplier { Id = 3, Name = "Gulf Bunkers LLC", ContactEmail = "ops@gulfbunkers.ae", ContactPhone = "+97141234567", Country = "UAE", CreatedAt = now }
        );

        modelBuilder.Entity<BunkerRequest>().HasData(
            new BunkerRequest { Id = 1, VesselId = 1, ProductId = 1, LocationId = 1, Quantity = 500, ETA = new DateTime(2024, 2, 15, 0, 0, 0, DateTimeKind.Utc), Status = RequestStatus.Quoted, CreatedAt = now, UpdatedAt = now },
            new BunkerRequest { Id = 2, VesselId = 2, ProductId = 2, LocationId = 2, Quantity = 200, ETA = new DateTime(2024, 2, 20, 0, 0, 0, DateTimeKind.Utc), Status = RequestStatus.Open, CreatedAt = now, UpdatedAt = now },
            new BunkerRequest { Id = 3, VesselId = 3, ProductId = 3, LocationId = 3, Quantity = 750, ETA = new DateTime(2024, 2, 25, 0, 0, 0, DateTimeKind.Utc), Status = RequestStatus.Ordered, CreatedAt = now, UpdatedAt = now },
            new BunkerRequest { Id = 4, VesselId = 1, ProductId = 2, LocationId = 2, Quantity = 300, ETA = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc), Status = RequestStatus.Draft, CreatedAt = now, UpdatedAt = now }
        );

        modelBuilder.Entity<SupplierQuote>().HasData(
            new SupplierQuote { Id = 1, BunkerRequestId = 1, SupplierId = 1, Price = 620m, Currency = "USD", ValidUntil = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc), Notes = "Price includes delivery", CreatedAt = now },
            new SupplierQuote { Id = 2, BunkerRequestId = 1, SupplierId = 2, Price = 615m, Currency = "USD", ValidUntil = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc), Notes = "Best price guaranteed", CreatedAt = now },
            new SupplierQuote { Id = 3, BunkerRequestId = 3, SupplierId = 3, Price = 645m, Currency = "USD", ValidUntil = new DateTime(2024, 2, 20, 0, 0, 0, DateTimeKind.Utc), Notes = "Fujairah ex-wharf", CreatedAt = now }
        );

        modelBuilder.Entity<BunkerOrder>().HasData(
            new BunkerOrder { Id = 1, BunkerRequestId = 3, SupplierQuoteId = 3, OrderDate = now, TotalAmount = 483750m, Currency = "USD", Status = OrderStatus.Confirmed, Notes = "Order confirmed", CreatedAt = now }
        );

        // Pre-computed BCrypt hash for "Admin@123"
        modelBuilder.Entity<AdminUser>().HasData(
            new AdminUser { Id = 1, Username = "admin", PasswordHash = "$2a$11$aQqFSwkbtyVU0muddz2zdupUDHYJvFqafKC27g3yr.9EMhVZAAGjm", FullName = "System Administrator", CreatedAt = now }
        );
    }
}
