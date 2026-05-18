using Commerce.Application.Products.GetProductById;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class GetProductByIdHandlerTests
{
    [Fact]
    public void Handle_WhenProductExists_ShouldReturnResult()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductByIdHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var result = handler.Handle(new GetProductByIdQuery(product.Id));

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Description, result.Description);
        Assert.Equal(product.Price, result.Price);
        Assert.Equal(product.Currency, result.Currency);
        Assert.Equal(product.IsActive, result.IsActive);
        Assert.Equal(product.CreatedAt, result.CreatedAt);
    }

    [Fact]
    public void Handle_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductByIdHandler(repository);

        var result = handler.Handle(new GetProductByIdQuery(Guid.NewGuid()));

        Assert.Null(result);
    }
}
