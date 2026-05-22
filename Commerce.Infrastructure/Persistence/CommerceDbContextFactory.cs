using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Commerce.Infrastructure.Persistence;

public sealed class CommerceDbContextFactory : IDesignTimeDbContextFactory<CommerceDbContext>
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=commerce;Username=postgres;Password=postgres";

    public CommerceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CommerceDbContext>();
        optionsBuilder.UseNpgsql(ConnectionString);

        return new CommerceDbContext(optionsBuilder.Options);
    }
}
