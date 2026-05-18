namespace Commerce.Application.Products.GetProducts;

public sealed class GetProductsHandler(IProductRepository repository)
{
    public IReadOnlyCollection<GetProductsResult> Handle()
    {
        return repository
            .GetAll()
            .Select(product => new GetProductsResult(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Currency,
                product.IsActive,
                product.CreatedAt))
            .ToList();
    }
}
