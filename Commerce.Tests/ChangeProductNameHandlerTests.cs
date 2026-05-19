using Commerce.Application.Products.ChangeProductName;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class ChangeProductNameHandlerTests
{
    [Fact]
    public void Handle_WhenProductExists_ShouldChangeName()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductNameHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        handler.Handle(new ChangeProductNameCommand(product.Id, "Keyboard"));

        Assert.Equal("Keyboard", product.Name);
    }

    [Fact]
    public void Handle_WhenProductExists_ShouldReturnResultWithNewName()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductNameHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var result = handler.Handle(new ChangeProductNameCommand(product.Id, "Keyboard"));

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal("Keyboard", result.Name);
    }

    [Fact]
    public void Handle_WhenProductExists_ShouldUpdateUpdatedAt()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductNameHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        var previousUpdatedAt = product.UpdatedAt;
        repository.Add(product);

        Thread.Sleep(1);
        var result = handler.Handle(new ChangeProductNameCommand(product.Id, "Keyboard"));

        Assert.NotNull(result);
        Assert.True(result.UpdatedAt > previousUpdatedAt);
        Assert.Equal(product.UpdatedAt, result.UpdatedAt);
    }

    [Fact]
    public void Handle_WhenProductDoesNotExist_ShouldReturnNull()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductNameHandler(repository);

        var result = handler.Handle(new ChangeProductNameCommand(Guid.NewGuid(), "Keyboard"));

        Assert.Null(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Handle_WithBlankName_ShouldThrowException(string newName)
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductNameHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var exception = Assert.Throws<ArgumentException>(() =>
            handler.Handle(new ChangeProductNameCommand(product.Id, newName)));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Handle_WithShortName_ShouldThrowException()
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductNameHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            handler.Handle(new ChangeProductNameCommand(product.Id, "AB")));

        Assert.Equal("name", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("AB")]
    public void Handle_WithInvalidName_ShouldKeepOldName(string newName)
    {
        var repository = new FakeProductRepository();
        var handler = new ChangeProductNameHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        Assert.ThrowsAny<ArgumentException>(() =>
            handler.Handle(new ChangeProductNameCommand(product.Id, newName)));

        Assert.Equal("Laptop", product.Name);
    }
}
