namespace Commerce.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private Product()
    {
    }
    private Product(Guid id, string name, string description, decimal price, string currency, bool isActive, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }
    public static Product Create(Guid id, string name, string description, decimal price, string currency, DateTimeOffset createdAt)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("İsim boş olamaz", nameof(name));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency boş olamaz", nameof(currency));
        if (name.Length < 3)
            throw new ArgumentOutOfRangeException(nameof(name), "İsim 3 karakterden az olamaz");
        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Değer 0 ve negatif olamaz");
        Product product = new(id, name, description, price, currency, true, createdAt);
        return product;
    }
    public static Product Create(string name, string description, decimal price, string currency)
    {
        return Create(Guid.NewGuid(), name, description, price, currency, DateTimeOffset.UtcNow);
    }
    public void ChangePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Değer 0 ve negatif olamaz");
        Price = price;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("İsim boş olamaz", nameof(name));
        if (name.Length < 3)
            throw new ArgumentOutOfRangeException(nameof(name), "İsim 3 karakterden az olamaz");
        Name = name;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void ChangeDescription(string description)
    {
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
