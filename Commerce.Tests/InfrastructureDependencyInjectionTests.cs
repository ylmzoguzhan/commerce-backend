using Commerce.Application.Products;
using Commerce.Infrastructure;
using Commerce.Infrastructure.Products;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Commerce.Tests;

public class InfrastructureDependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_WithoutOptions_ShouldUseInMemoryProductRepository()
    {
        var services = new ServiceCollection();

        services.AddInfrastructure();

        using var serviceProvider = services.BuildServiceProvider();
        var repository = serviceProvider.GetRequiredService<IProductRepository>();
        Assert.IsType<InMemoryProductRepository>(repository);
    }

    [Fact]
    public void AddInfrastructure_WithEfRepositoryButWithoutConnectionString_ShouldUseInMemoryProductRepository()
    {
        var services = new ServiceCollection();

        services.AddInfrastructure(useEfRepository: true);

        using var serviceProvider = services.BuildServiceProvider();
        var repository = serviceProvider.GetRequiredService<IProductRepository>();
        Assert.IsType<InMemoryProductRepository>(repository);
    }

    [Fact]
    public void AddInfrastructure_WithEfRepositoryAndConnectionString_ShouldUseEfProductRepository()
    {
        var services = new ServiceCollection();

        services.AddInfrastructure(
            connectionString: "Host=localhost;Port=5432;Database=commerce;Username=postgres;Password=postgres",
            useEfRepository: true);

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        Assert.IsType<EfProductRepository>(repository);
    }
}
