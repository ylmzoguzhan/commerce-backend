using Commerce.Application.Products.GetProducts;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class GetProductsHandlerTests
{
    [Fact]
    public void Handle_WhenRepositoryIsEmpty_ShouldReturnEmptyCollection()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);

        var result = handler.Handle();

        Assert.Empty(result);
    }

    [Fact]
    public void Handle_WhenProductsExist_ShouldReturnProducts()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var firstProduct = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        var secondProduct = Product.Create("Phone", "Smart phone", 30000, "TRY");
        repository.Add(firstProduct);
        repository.Add(secondProduct);

        var result = handler.Handle();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Handle_WhenProductsExist_ShouldMapProductFields()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var result = Assert.Single(handler.Handle());

        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Description, result.Description);
        Assert.Equal(product.Price, result.Price);
        Assert.Equal(product.Currency, result.Currency);
        Assert.Equal(product.IsActive, result.IsActive);
        Assert.Equal(product.CreatedAt, result.CreatedAt);
    }
}
