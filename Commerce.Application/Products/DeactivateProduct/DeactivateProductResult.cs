namespace Commerce.Application.Products.DeactivateProduct;

public record DeactivateProductResult(Guid Id, bool IsActive, DateTimeOffset UpdatedAt);
