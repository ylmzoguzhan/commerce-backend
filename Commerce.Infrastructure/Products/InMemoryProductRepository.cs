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

    public Product? GetById(Guid id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }

    public IReadOnlyCollection<Product> GetPaged(int page, int pageSize, string sortBy, string sortDirection, string? search, decimal? minPrice, decimal? maxPrice, bool? isActive)
    {
        var products = ApplyFilters(search, minPrice, maxPrice, isActive);

        products = sortBy.ToLowerInvariant() switch
        {
            "createdat" => Sort(products, product => product.CreatedAt, sortDirection),
            "name" => Sort(products, product => product.Name, sortDirection),
            "price" => Sort(products, product => product.Price, sortDirection),
            _ => Sort(products, product => product.CreatedAt, "desc")
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


    public int Count(string? search, decimal? minPrice, decimal? maxPrice, bool? isActive)
    {
        return ApplyFilters(search, minPrice, maxPrice, isActive).Count();
    }

    private IEnumerable<Product> ApplyFilters(string? search, decimal? minPrice, decimal? maxPrice, bool? isActive)
    {
        var products = _products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            products = products.Where(product =>
                product.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                product.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (minPrice.HasValue)
            products = products.Where(product => product.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            products = products.Where(product => product.Price <= maxPrice.Value);

        if (isActive.HasValue)
            products = products.Where(product => product.IsActive == isActive.Value);

        return products;
    }
}
