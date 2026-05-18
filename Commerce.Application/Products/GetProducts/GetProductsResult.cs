namespace Commerce.Application.Products.GetProducts;

public sealed record GetProductsResult(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    bool IsActive,
    DateTimeOffset CreatedAt);
