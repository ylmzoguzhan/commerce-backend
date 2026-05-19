namespace Commerce.Application.Products.ChangeProductPrice;

public sealed record ChangeProductPriceCommand(Guid ProductId, decimal NewPrice);