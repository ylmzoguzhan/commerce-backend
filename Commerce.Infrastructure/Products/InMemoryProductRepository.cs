using Commerce.Application.Products;
using Commerce.Domain;

namespace Commerce.Infrastructure.Products;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];

    public void Add(Product product)
    {
        _products.Add(product);
    }

    public IReadOnlyCollection<Product> GetAll()
    {
        return _products.ToList();
    }

    public Product? GetById(Guid id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }
}
