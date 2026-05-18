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

    public Product? GetById(Guid Id)
    {
        return _products.FirstOrDefault(op => op.Id == Id);
    }
}
