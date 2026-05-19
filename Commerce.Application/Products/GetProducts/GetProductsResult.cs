namespace Commerce.Application.Products.GetProducts;

public sealed record GetProductsResult(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    bool IsActive,
    DateTimeOffset CreatedAt);
public sealed record GetProductsResponse(
    IReadOnlyCollection<GetProductsResult> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);