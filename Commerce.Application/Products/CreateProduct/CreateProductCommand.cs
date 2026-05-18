namespace Commerce.Application.Products.CreateProduct;

public record CreateProductCommand(string Name, string Description, decimal Price, string Currency);
