namespace Commerce.Application.Products.ChangeProductDescription;

public record ChangeProductDescriptionResult(Guid Id, string Description, DateTimeOffset UpdatedAt);
