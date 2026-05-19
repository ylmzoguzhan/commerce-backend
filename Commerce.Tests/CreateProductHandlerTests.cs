using Commerce.Application;
using Commerce.Application.Products;
using Commerce.Application.Products.CreateProduct;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class CreateProductHandlerTests
{
    private static readonly DateTimeOffset ClockTime = new(2026, 5, 19, 10, 30, 0, TimeSpan.Zero);

    [Fact]
    public void Handle_WithValidCommand_ShouldCreateProductAndReturnResult()
    {
        var repository = new FakeProductRepository();
        var handler = new CreateProductHandler(repository, new FakeClock(ClockTime));
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
        Assert.Equal(ClockTime, result.CreatedAt);

        var savedProduct = Assert.Single(repository.Products);
        Assert.Equal(result.Id, savedProduct.Id);
        Assert.Equal(command.Name, savedProduct.Name);
        Assert.Equal(command.Description, savedProduct.Description);
        Assert.Equal(command.Price, savedProduct.Price);
        Assert.Equal(command.Currency, savedProduct.Currency);
        Assert.Equal(ClockTime, savedProduct.CreatedAt);
        Assert.Equal(ClockTime, savedProduct.UpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Handle_WithInvalidName_ShouldThrowException(string? name)
    {
        var repository = new FakeProductRepository();
        var handler = new CreateProductHandler(repository, new FakeClock(ClockTime));
        var command = new CreateProductCommand(
            Name: name!,
            Description: "Su geçirmez",
            Price: 100,
            Currency: "TL");

        var exception = Assert.Throws<ArgumentException>(() => handler.Handle(command));

        Assert.StartsWith("İsim boş olamaz", exception.Message);
        Assert.Equal("name", exception.ParamName);
        Assert.Empty(repository.Products);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Handle_WithInvalidPrice_ShouldThrowException(decimal price)
    {
        var repository = new FakeProductRepository();
        var handler = new CreateProductHandler(repository, new FakeClock(ClockTime));
        var command = new CreateProductCommand(
            Name: "Akıllı Saat",
            Description: "Su geçirmez",
            Price: price,
            Currency: "TL");

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => handler.Handle(command));

        Assert.Contains("Değer 0 ve negatif olamaz", exception.Message);
        Assert.Equal("price", exception.ParamName);
        Assert.Empty(repository.Products);
    }

    private sealed class FakeClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }
}
