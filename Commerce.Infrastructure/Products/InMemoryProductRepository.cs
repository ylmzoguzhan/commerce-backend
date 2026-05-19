using Commerce.Application.Products;
using Commerce.Domain;

namespace Commerce.Infrastructure.Products;

public sealed class InMemoryProductRepository : IProductRepository
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

    public IReadOnlyCollection<Product> GetPaged(int page, int pageSize, string sortBy, string sortDirection)
    {
        var products = sortBy.ToLowerInvariant() switch
        {
            "createdat" => Sort(_products, product => product.CreatedAt, sortDirection),
            "name" => Sort(_products, product => product.Name, sortDirection),
            "price" => Sort(_products, product => product.Price, sortDirection),
            _ => Sort(_products, product => product.CreatedAt, "desc")
        };

        return products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    private static IEnumerable<Product> Sort<TKey>(
        IEnumerable<Product> products,
        Func<Product, TKey> keySelector,
        string sortDirection)
    {
        return sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? products.OrderByDescending(keySelector)
            : products.OrderBy(keySelector);
    }
}
