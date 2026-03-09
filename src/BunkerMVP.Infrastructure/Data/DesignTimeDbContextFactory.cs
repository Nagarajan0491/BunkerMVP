using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BunkerMVP.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BunkerDbContext>
{
    public BunkerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BunkerDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=BunkerMVP;Username=postgres;Password=postgres");
        return new BunkerDbContext(optionsBuilder.Options);
    }
}
