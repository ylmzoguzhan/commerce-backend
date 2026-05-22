using Commerce.Domain;
using Commerce.Infrastructure.Persistence;
using Commerce.Infrastructure.Products;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commerce.Tests;

public class EfProductRepositoryTests
{
    [Fact]
    public void Add_ShouldPersistProduct()
    {
        using var dbContext = CreateDbContext();
        var repository = new EfProductRepository(dbContext);
        var product = CreateProduct("Laptop", "Gaming laptop", 45000, createdAt: DateTimeOffset.UtcNow);

        repository.Add(product);

        var savedProduct = Assert.Single(dbContext.Products);
        Assert.Equal(product.Id, savedProduct.Id);
        Assert.Equal(product.Name, savedProduct.Name);
    }

    [Fact]
    public void GetById_WhenProductExists_ShouldReturnProduct()
    {
        using var dbContext = CreateDbContext();
        var repository = new EfProductRepository(dbContext);
        var product = CreateProduct("Laptop", "Gaming laptop", 45000, createdAt: DateTimeOffset.UtcNow);
        Seed(dbContext, product);

        var result = repository.GetById(product.Id);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);
    }

    [Fact]
    public void GetById_WhenProductDoesNotExist_ShouldReturnNull()
    {
        using var dbContext = CreateDbContext();
        var repository = new EfProductRepository(dbContext);

        var result = repository.GetById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public void GetPaged_ShouldApplyFilteringSortingAndPagination()
    {
        using var dbContext = CreateDbContext();
        var repository = new EfProductRepository(dbContext);
        var cheapestProduct = CreateProduct(
            "Mouse",
            "Wireless gaming mouse",
            1000,
            createdAt: new DateTimeOffset(2026, 5, 19, 10, 0, 0, TimeSpan.Zero));
        var middleProduct = CreateProduct(
            "Phone",
            "Gaming phone",
            30000,
            createdAt: new DateTimeOffset(2026, 5, 19, 11, 0, 0, TimeSpan.Zero));
        var expensiveProduct = CreateProduct(
            "Laptop",
            "Gaming laptop",
            45000,
            createdAt: new DateTimeOffset(2026, 5, 19, 12, 0, 0, TimeSpan.Zero));
        var unrelatedProduct = CreateProduct(
            "Chair",
            "Office chair",
            7000,
            createdAt: new DateTimeOffset(2026, 5, 19, 13, 0, 0, TimeSpan.Zero));
        Seed(dbContext, cheapestProduct, middleProduct, expensiveProduct, unrelatedProduct);

        var result = Assert.Single(repository.GetPaged(
            page: 2,
            pageSize: 1,
            sortBy: "price",
            sortDirection: "desc",
            search: "gaming",
            minPrice: null,
            maxPrice: null,
            isActive: true));

        Assert.Equal(middleProduct.Id, result.Id);
    }

    [Fact]
    public void Count_ShouldReturnFilteredProductCount()
    {
        using var dbContext = CreateDbContext();
        var repository = new EfProductRepository(dbContext);
        var activeProduct = CreateProduct("Laptop", "Gaming laptop", 45000, createdAt: DateTimeOffset.UtcNow);
        var inactiveProduct = CreateProduct("Phone", "Gaming phone", 30000, createdAt: DateTimeOffset.UtcNow);
        inactiveProduct.Deactivate();
        var unrelatedProduct = CreateProduct("Chair", "Office chair", 7000, createdAt: DateTimeOffset.UtcNow);
        Seed(dbContext, activeProduct, inactiveProduct, unrelatedProduct);

        var count = repository.Count(
            search: "gaming",
            minPrice: null,
            maxPrice: null,
            isActive: true);

        Assert.Equal(1, count);
    }

    private static CommerceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<CommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CommerceDbContext(options);
    }

    private static Product CreateProduct(
        string name,
        string description,
        decimal price,
        DateTimeOffset createdAt)
    {
        return Product.Create(Guid.NewGuid(), name, description, price, "TRY", createdAt);
    }

    private static void Seed(CommerceDbContext dbContext, params Product[] products)
    {
        dbContext.Products.AddRange(products);
        dbContext.SaveChanges();
    }
}
