using Commerce.Application.Products.ChangeProductPrice;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class ChangeProductPriceHandlerTests
{
    [Fact]
    public void Handle_WhenProductExists_ShouldChangePrice()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductPriceHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        handler.Handle(new ChangeProductPriceCommand(product.Id, 50000));

        Assert.Equal(50000, product.Price);
    }

    [Fact]
    public void Handle_WhenProductExists_ShouldReturnResultWithNewPrice()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductPriceHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var result = handler.Handle(new ChangeProductPriceCommand(product.Id, 50000));

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(50000, result.Price);
    }

    [Fact]
    public void Handle_WhenProductExists_ShouldUpdateUpdatedAt()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductPriceHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        var previousUpdatedAt = product.UpdatedAt;
        repository.Add(product);

        Thread.Sleep(1);
        var result = handler.Handle(new ChangeProductPriceCommand(product.Id, 50000));

        Assert.NotNull(result);
        Assert.True(result.UpdatedAt > previousUpdatedAt);
        Assert.Equal(product.UpdatedAt, result.UpdatedAt);
    }

    [Fact]
    public void Handle_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductPriceHandler(repository);

        var result = handler.Handle(new ChangeProductPriceCommand(Guid.NewGuid(), 50000));

        Assert.Null(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Handle_WithInvalidPrice_ShouldThrowException(decimal newPrice)
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductPriceHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            handler.Handle(new ChangeProductPriceCommand(product.Id, newPrice)));

        Assert.Equal("price", exception.ParamName);
    }

    [Fact]
    public void Handle_WithInvalidPrice_ShouldKeepOldPrice()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductPriceHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            handler.Handle(new ChangeProductPriceCommand(product.Id, -1)));

        Assert.Equal(45000, product.Price);
    }
}
