using Commerce.Domain;

namespace Commerce.Application.Products.CreateProduct;

public sealed class CreateProductHandler
{
    public CreateProductResult Handle(CreateProductCommand command)
    {
        var product = Product.Create(
            command.Name,
            command.Description,
            command.Price,
            command.Currency);

        return new CreateProductResult(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Currency,
            product.IsActive,
            product.CreatedAt);
    }
}
