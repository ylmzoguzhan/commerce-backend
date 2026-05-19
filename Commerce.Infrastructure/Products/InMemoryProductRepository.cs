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

    public int Count()
    {
        return _products.Count();
    }

    public Product? GetById(Guid id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }

    public IReadOnlyCollection<Product> GetPaged(int page, int pageSize)
    {
        return _products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }
}
