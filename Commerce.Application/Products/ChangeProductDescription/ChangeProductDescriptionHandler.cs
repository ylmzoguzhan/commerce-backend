namespace Commerce.Application.Products.ChangeProductDescription;

public sealed class ChangeProductDescriptionHandler(IProductRepository repository)
{
    public ChangeProductDescriptionResult? Handle(ChangeProductDescriptionCommand command)
    {
        var product = repository.GetById(command.Id);
        if (product is null)
            return null;

        product.ChangeDescription(command.Description);

        return new ChangeProductDescriptionResult(product.Id, product.Description, product.UpdatedAt);
    }
}
