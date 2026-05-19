namespace Commerce.Application.Products.DeactivateProduct;

public sealed class DeactivateProductHandler(IProductRepository repository)
{
    public DeactivateProductResult? Handle(DeactivateProductCommand command)
    {
        var product = repository.GetById(command.Id);
        if (product is null) return null;
        product.Deactivate();
        return new DeactivateProductResult(product.Id, product.IsActive, product.UpdatedAt);
    }
}
