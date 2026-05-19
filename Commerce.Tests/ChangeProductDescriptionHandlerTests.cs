using Commerce.Application.Products.ChangeProductDescription;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class ChangeProductDescriptionHandlerTests
{
    [Fact]
    public void Handle_WhenProductExists_ShouldChangeDescription()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductDescriptionHandler(repository);
        var product = Product.Create("Laptop", "Old description", 45000, "TRY");
        repository.Add(product);

        handler.Handle(new ChangeProductDescriptionCommand(product.Id, "New description"));

        Assert.Equal("New description", product.Description);
    }

    [Fact]
    public void Handle_WhenProductExists_ShouldReturnResultWithNewDescription()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductDescriptionHandler(repository);
        var product = Product.Create("Laptop", "Old description", 45000, "TRY");
        repository.Add(product);

        var result = handler.Handle(new ChangeProductDescriptionCommand(product.Id, "New description"));

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal("New description", result.Description);
    }

    [Fact]
    public void Handle_WhenProductExists_ShouldUpdateUpdatedAt()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductDescriptionHandler(repository);
        var product = Product.Create("Laptop", "Old description", 45000, "TRY");
        var previousUpdatedAt = product.UpdatedAt;
        repository.Add(product);

        Thread.Sleep(1);
        var result = handler.Handle(new ChangeProductDescriptionCommand(product.Id, "New description"));

        Assert.NotNull(result);
        Assert.True(result.UpdatedAt > previousUpdatedAt);
        Assert.Equal(product.UpdatedAt, result.UpdatedAt);
    }

    [Fact]
    public void Handle_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductDescriptionHandler(repository);

        var result = handler.Handle(new ChangeProductDescriptionCommand(Guid.NewGuid(), "New description"));

        Assert.Null(result);
    }

    [Fact]
    public void Handle_WithEmptyDescription_ShouldChangeDescription()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductDescriptionHandler(repository);
        var product = Product.Create("Laptop", "Old description", 45000, "TRY");
        repository.Add(product);

        var result = handler.Handle(new ChangeProductDescriptionCommand(product.Id, ""));

        Assert.NotNull(result);
        Assert.Equal("", product.Description);
        Assert.Equal("", result.Description);
    }
}
