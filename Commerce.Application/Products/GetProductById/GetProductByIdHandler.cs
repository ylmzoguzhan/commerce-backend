using Commerce.Application.Products;

namespace Commerce.Application.Products.GetProductById;

public sealed class GetProductByIdHandler(IProductRepository repository)
{
    public GetProductByIdResult? Handle(GetProductByIdQuery query)
    {
        var product = repository.GetById(query.Id);

        return product is null
            ? null
            : new GetProductByIdResult(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Currency,
                product.IsActive,
                product.CreatedAt);
    }
}
