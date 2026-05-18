namespace Commerce.Application.Products.GetProductById;

public sealed record GetProductByIdResult(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    bool IsActive,
    DateTimeOffset CreatedAt);


