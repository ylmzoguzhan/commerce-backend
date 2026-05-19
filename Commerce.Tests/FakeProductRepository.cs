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

    public IReadOnlyCollection<Product> GetPaged(int page, int pageSize, string sortBy, string sortDirection)
    {
        var products = sortBy.ToLowerInvariant() switch
        {
            "createdat" => Sort(Products, product => product.CreatedAt, sortDirection),
            "name" => Sort(Products, product => product.Name, sortDirection),
            "price" => Sort(Products, product => product.Price, sortDirection),
            _ => Sort(Products, product => product.CreatedAt, "desc")
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
