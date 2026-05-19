using Commerce.Application.Products.ActivateProduct;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class ActivateProductHandlerTests
{
    [Fact]
    public void Handle_WhenProductExists_ShouldActivateProductAndUpdateTimestamp()
    {
        var repository = new FakeProductRepository();
        var handler = new ActivateProductHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        product.Deactivate();
        var oldDate = product.UpdatedAt;
        repository.Add(product);

        var result = handler.Handle(new ActivateProductCommand(product.Id));

        Assert.NotNull(result);
        Assert.True(product.IsActive);
        Assert.True(result.IsActive);
        Assert.Equal(product.Id, result.Id);
        Assert.True(product.UpdatedAt > oldDate);
        Assert.Equal(product.UpdatedAt, result.UpdatedAt);
    }

    [Fact]
    public void Handle_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var handler = new ActivateProductHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var result = handler.Handle(new ActivateProductCommand(Guid.NewGuid()));

        Assert.Null(result);
    }
}
