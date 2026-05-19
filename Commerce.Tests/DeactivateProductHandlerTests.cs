using Commerce.Application.Products.DeactivateProduct;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class DeactivateProductHandlerTests
{
    [Fact]
    public void Handle_WhenProductExists_ShouldDeactivateProductAndUpdateTimestamp()
    {
        var repository = new FakeProductRepository();
        var handler = new DeactivateProductHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        var oldDate = product.UpdatedAt;
        repository.Add(product);

        var result = handler.Handle(new DeactivateProductCommand(product.Id));

        Assert.NotNull(result);
        Assert.False(product.IsActive);
        Assert.False(result.IsActive);
        Assert.Equal(product.Id, result.Id);
        Assert.True(product.UpdatedAt > oldDate);
        Assert.Equal(product.UpdatedAt, result.UpdatedAt);
    }

    [Fact]
    public void Handle_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var handler = new DeactivateProductHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var result = handler.Handle(new DeactivateProductCommand(Guid.NewGuid()));

        Assert.Null(result);
    }
}
