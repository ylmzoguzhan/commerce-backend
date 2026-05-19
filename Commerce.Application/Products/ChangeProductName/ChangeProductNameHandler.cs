namespace Commerce.Application.Products.ChangeProductName;

public sealed class ChangeProductNameHandler(IProductRepository repository)
{
    public ChangeProductNameResult? Handle(ChangeProductNameCommand command)
    {
        var product = repository.GetById(command.Id);
        if (product is null)
            return null;
        product.Rename(command.NewName);
        return new ChangeProductNameResult(product.Id, product.Name, product.UpdatedAt);
    }
}
