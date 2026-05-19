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

    [Fact]
    public void Create_WithProvidedId_ShouldUseProvidedId()
    {
        var id = Guid.NewGuid();

        var product = Product.Create(id, "Laptop", "Oyun Bilgisayarı", 15000, "TL", DateTimeOffset.UtcNow);

        Assert.Equal(id, product.Id);
    }

    [Fact]
    public void Create_WithProvidedCreatedAt_ShouldUseProvidedCreatedAt()
    {
        var createdAt = new DateTimeOffset(2026, 5, 19, 10, 30, 0, TimeSpan.Zero);

        var product = Product.Create(Guid.NewGuid(), "Laptop", "Oyun Bilgisayarı", 15000, "TL", createdAt);

        Assert.Equal(createdAt, product.CreatedAt);
    }

    [Fact]
    public void Create_WithProvidedCreatedAt_ShouldSetUpdatedAtToCreatedAt()
    {
        var createdAt = new DateTimeOffset(2026, 5, 19, 10, 30, 0, TimeSpan.Zero);

        var product = Product.Create(Guid.NewGuid(), "Laptop", "Oyun Bilgisayarı", 15000, "TL", createdAt);

        Assert.Equal(createdAt, product.UpdatedAt);
    }

    [Fact]
    public void Create_ValidData_ShouldSetCreatedAtAndUpdatedAtToSameValue()
    {
        var product = Product.Create("Laptop", "Oyun Bilgisayarı", 15000, "TL");

        Assert.Equal(product.CreatedAt, product.UpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_EmptyName_ShouldThrowException(string? name)
    {
        var exception = Assert.Throws<ArgumentException>(() => Product.Create(name!, "Açıklama", 100, "TL"));

        Assert.StartsWith("İsim boş olamaz", exception.Message);
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Create_ShortName_ShouldThrowException()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => Product.Create("Ab", "Açıklama", 100, "TL"));

        Assert.Contains("İsim 3 karakterden az olamaz", exception.Message);
        Assert.Equal("name", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_ZeroOrNegativePrice_ShouldThrowException(decimal price)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => Product.Create("Laptop", "Açıklama", price, "TL"));

        Assert.Contains("Değer 0 ve negatif olamaz", exception.Message);
        Assert.Equal("price", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_EmptyCurrency_ShouldThrowException(string? currency)
    {
        var exception = Assert.Throws<ArgumentException>(() => Product.Create("Laptop", "Açıklama", 100, currency!));

        Assert.StartsWith("Currency boş olamaz", exception.Message);
        Assert.Equal("currency", exception.ParamName);
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

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => product.ChangePrice(invalidPrice));

        Assert.Contains("Değer 0 ve negatif olamaz", exception.Message);
        Assert.Equal("price", exception.ParamName);
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

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => product.Rename(shortName));

        Assert.Contains("İsim 3 karakterden az olamaz", exception.Message);
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void ChangeDescription_ValidDescription_ShouldUpdateDescription()
    {
        var product = Product.Create("Laptop", "Old description", 100, "TL");

        product.ChangeDescription("New description");

        Assert.Equal("New description", product.Description);
    }

    [Fact]
    public void ChangeDescription_EmptyDescription_ShouldUpdateDescription()
    {
        var product = Product.Create("Laptop", "Old description", 100, "TL");

        product.ChangeDescription("");

        Assert.Equal("", product.Description);
    }

    [Fact]
    public void ChangeDescription_ValidDescription_ShouldUpdateTimestamp()
    {
        var product = Product.Create("Laptop", "Old description", 100, "TL");
        var previousUpdatedAt = product.UpdatedAt;

        Thread.Sleep(1);
        product.ChangeDescription("New description");

        Assert.True(product.UpdatedAt > previousUpdatedAt);
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
