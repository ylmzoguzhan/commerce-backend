namespace Commerce.Application.Products.CreateProduct;

public record CreateProductResult(Guid Id, string Name, string Description, decimal Price, string Currency, bool IsActive, DateTimeOffset CreatedAt);
