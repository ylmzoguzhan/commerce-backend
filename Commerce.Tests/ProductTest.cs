using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class ProductTests
{
    [Fact]
    public void Create_ValidData_ShouldCreateActiveProduct()
    {
        var product = Product.Create("Laptop", "Oyun Bilgisayarı", 15000, "TL");

        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal("Laptop", product.Name);
        Assert.Equal("Oyun Bilgisayarı", product.Description);
        Assert.Equal(15000, product.Price);
        Assert.Equal("TL", product.Currency);
        Assert.True(product.IsActive);
        Assert.True(product.CreatedAt <= DateTimeOffset.UtcNow);
        Assert.True(product.UpdatedAt <= DateTimeOffset.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_EmptyName_ShouldThrowException(string? name)
    {
        var exception = Assert.Throws<Exception>(() => Product.Create(name!, "Açıklama", 100, "TL"));

        Assert.Equal("İsim boş olamaz", exception.Message);
    }

    [Fact]
    public void Create_ShortName_ShouldThrowException()
    {
        var exception = Assert.Throws<Exception>(() => Product.Create("Ab", "Açıklama", 100, "TL"));

        Assert.Equal("İsim 3 karakterden az olamaz", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_ZeroOrNegativePrice_ShouldThrowException(decimal price)
    {
        var exception = Assert.Throws<Exception>(() => Product.Create("Laptop", "Açıklama", price, "TL"));

        Assert.Equal("Değer 0 ve negatif olamaz", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_EmptyCurrency_ShouldThrowException(string? currency)
    {
        var exception = Assert.Throws<Exception>(() => Product.Create("Laptop", "Açıklama", 100, currency!));

        Assert.Equal("Currency boş olamaz", exception.Message);
    }

    [Fact]
    public void ChangePrice_ValidPrice_ShouldUpdatePriceAndUpdatedAt()
    {
        var product = Product.Create("Laptop", "Oyun Bilgisayarı", 15000, "TL");
        var previousUpdatedAt = product.UpdatedAt;
        decimal newPrice = 18000;

        Thread.Sleep(1);
        product.ChangePrice(newPrice);

        Assert.Equal(newPrice, product.Price);
        Assert.True(product.UpdatedAt > previousUpdatedAt);
    }

    [Fact]
    public void ChangePrice_ZeroOrNegativePrice_ShouldThrowException()
    {
        var product = Product.Create("Laptop", "Oyun Bilgisayarı", 15000, "TL");
        decimal invalidPrice = -50;

        var exception = Assert.Throws<Exception>(() => product.ChangePrice(invalidPrice));

        Assert.Equal("Değer 0 ve negatif olamaz", exception.Message);
    }
    [Fact]
    public void Rename_ValidName_ShouldUpdateName()
    {
        var product = Product.Create("Eski İsim", "Açıklama", 100, "TL");
        var previousUpdatedAt = product.UpdatedAt;
        string newName = "Yeni Ürün İsmi";

        Thread.Sleep(1);
        product.Rename(newName);

        Assert.Equal(newName, product.Name);
        Assert.True(product.UpdatedAt > previousUpdatedAt);
    }

    [Fact]
    public void Rename_ShortName_ShouldThrowException()
    {
        var product = Product.Create("Eski İsim", "Açıklama", 100, "TL");
        string shortName = "Ab";

        var exception = Assert.Throws<Exception>(() => product.Rename(shortName));

        Assert.Equal("İsim 3 karakterden az olamaz", exception.Message);
    }

    [Fact]
    public void Deactivate_ActiveProduct_ShouldMakeProductInactiveAndUpdateTimestamp()
    {
        var product = Product.Create("Laptop", "Oyun Bilgisayarı", 15000, "TL");
        var previousUpdatedAt = product.UpdatedAt;

        Thread.Sleep(1);
        product.Deactivate();

        Assert.False(product.IsActive);
        Assert.True(product.UpdatedAt > previousUpdatedAt);
    }

    [Fact]
    public void Activate_InactiveProduct_ShouldMakeProductActiveAndUpdateTimestamp()
    {
        var product = Product.Create("Laptop", "Oyun Bilgisayarı", 15000, "TL");
        product.Deactivate();
        var previousUpdatedAt = product.UpdatedAt;

        Thread.Sleep(1);
        product.Activate();

        Assert.True(product.IsActive);
        Assert.True(product.UpdatedAt > previousUpdatedAt);
    }
}
