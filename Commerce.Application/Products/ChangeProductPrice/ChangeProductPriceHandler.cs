namespace Commerce.Application.Products.ChangeProductPrice;

public sealed class ChangeProductPriceHandler(IProductRepository repository)
{
    public ChangeProductPriceResult? Handle(ChangeProductPriceCommand command)
    {
        var product = repository.GetById(command.ProductId);

        if (product is null)
            return null;

        product.ChangePrice(command.NewPrice);

        return new ChangeProductPriceResult(product.Id, product.Price, product.UpdatedAt);
    }
}
