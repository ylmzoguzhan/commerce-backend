namespace Commerce.Application.Products.ActivateProduct;

public record ActivateProductResult(Guid Id, bool IsActive, DateTimeOffset UpdatedAt);