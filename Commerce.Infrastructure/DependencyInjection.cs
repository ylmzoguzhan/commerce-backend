using Commerce.Application.Products;
using Commerce.Infrastructure.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Commerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();

        return services;
    }
}
