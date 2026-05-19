using Commerce.Application.Products;
using Commerce.Domain;

namespace Commerce.Application.Products.CreateProduct;

public sealed class CreateProductHandler(IProductRepository repository, IClock clock)
{
    public CreateProductResult Handle(CreateProductCommand command)
    {
        var product = Product.Create(
            Guid.NewGuid(),
            command.Name,
            command.Description,
            command.Price,
            command.Currency,
            clock.UtcNow);
        repository.Add(product);
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
