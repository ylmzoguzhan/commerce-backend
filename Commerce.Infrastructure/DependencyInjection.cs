using Commerce.Application;
using Commerce.Application.Products;
using Commerce.Infrastructure.Persistence;
using Commerce.Infrastructure.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Commerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString = null)
    {
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<CommerceDbContext>(options => options.UseNpgsql(connectionString));
        }

        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IIdGenerator, GuidIdGenerator>();
        return services;
    }
}
