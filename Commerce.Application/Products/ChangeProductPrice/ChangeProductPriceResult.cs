namespace Commerce.Application.Products.ChangeProductPrice;

public sealed record ChangeProductPriceResult(
    Guid Id,
    decimal Price,
    DateTimeOffset UpdatedAt);