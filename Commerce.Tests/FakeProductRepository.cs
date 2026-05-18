using Commerce.Application.Products;
using Commerce.Domain;

namespace Commerce.Tests;

public sealed class FakeProductRepository : IProductRepository
{
    public List<Product> Products { get; } = [];

    public void Add(Product product)
    {
        Products.Add(product);
    }

    public Product? GetById(Guid Id)
    {
        return Products.FirstOrDefault(op => op.Id == Id);
    }
}