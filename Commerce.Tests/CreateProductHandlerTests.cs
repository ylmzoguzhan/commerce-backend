using Commerce.Application.Products.CreateProduct;
using Xunit;

namespace Commerce.Tests;

public class CreateProductHandlerTests
{
    [Fact]
    public void Handle_WithValidCommand_ShouldCreateProductAndReturnResult()
    {
        var handler = new CreateProductHandler();
        var command = new CreateProductCommand(
            Name: "Kablosuz Kulaklık",
            Description: "Gürültü engelleyici özellikli",
            Price: 2500,
            Currency: "TL");

        var result = handler.Handle(command);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.Price, result.Price);
        Assert.Equal(command.Currency, result.Currency);
        Assert.True(result.IsActive);
        Assert.True(result.CreatedAt <= DateTimeOffset.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Handle_WithInvalidName_ShouldThrowException(string? name)
    {
        var handler = new CreateProductHandler();
        var command = new CreateProductCommand(
            Name: name!,
            Description: "Su geçirmez",
            Price: 100,
            Currency: "TL");

        var exception = Assert.Throws<Exception>(() => handler.Handle(command));

        Assert.Equal("İsim boş olamaz", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Handle_WithInvalidPrice_ShouldThrowException(decimal price)
    {
        var handler = new CreateProductHandler();
        var command = new CreateProductCommand(
            Name: "Akıllı Saat",
            Description: "Su geçirmez",
            Price: price,
            Currency: "TL");

        var exception = Assert.Throws<Exception>(() => handler.Handle(command));

        Assert.Equal("Değer 0 ve negatif olamaz", exception.Message);
    }
}
