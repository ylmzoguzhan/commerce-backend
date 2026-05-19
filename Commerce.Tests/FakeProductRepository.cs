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

    public int Count()
    {
        return Products.Count();
    }

    public Product? GetById(Guid id)
    {
        return Products.FirstOrDefault(product => product.Id == id);
    }

    public IReadOnlyCollection<Product> GetPaged(int page, int pageSize)
    {
        return Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }
}
