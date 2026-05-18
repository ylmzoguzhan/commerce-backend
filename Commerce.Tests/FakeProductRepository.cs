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

    public IReadOnlyCollection<Product> GetAll()
    {
        return Products.ToList();
    }

    public Product? GetById(Guid id)
    {
        return Products.FirstOrDefault(product => product.Id == id);
    }
}
