namespace Commerce.Application.Products.ActivateProduct;

public sealed class ActivateProductHandler(IProductRepository repository)
{
    public ActivateProductResult? Handle(ActivateProductCommand command)
    {
        var product = repository.GetById(command.Id);
        if (product is null) return null;
        product.Activate();
        return new ActivateProductResult(product.Id, product.IsActive, product.UpdatedAt);
    }
}
