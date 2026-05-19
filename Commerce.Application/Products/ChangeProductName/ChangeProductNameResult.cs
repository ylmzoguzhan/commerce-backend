namespace Commerce.Application.Products.ChangeProductName;

public record ChangeProductNameResult(Guid Id, string Name, DateTimeOffset UpdatedAt);
