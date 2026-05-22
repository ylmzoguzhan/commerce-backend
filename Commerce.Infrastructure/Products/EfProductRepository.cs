using Commerce.Application.Products;
using Commerce.Domain;
using Commerce.Infrastructure.Persistence;

namespace Commerce.Infrastructure.Products;

public sealed class EfProductRepository(CommerceDbContext dbContext) : IProductRepository
{
    public void Add(Product product)
    {
        dbContext.Products.Add(product);
        dbContext.SaveChanges();
    }

    public int Count(string? search, decimal? minPrice, decimal? maxPrice, bool? isActive)
    {
        return ApplyFilters(search, minPrice, maxPrice, isActive).Count();
    }

    public Product? GetById(Guid id)
    {
        return dbContext.Products.FirstOrDefault(product => product.Id == id);
    }

    public IReadOnlyCollection<Product> GetPaged(int page, int pageSize, string sortBy, string sortDirection, string? search, decimal? minPrice, decimal? maxPrice, bool? isActive)
    {
        var products = ApplyFilters(search, minPrice, maxPrice, isActive);

        products = ApplySorting(products, sortBy, sortDirection);

        return products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    private IQueryable<Product> ApplyFilters(string? search, decimal? minPrice, decimal? maxPrice, bool? isActive)
    {
        var products = dbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.ToLowerInvariant();
            products = products.Where(product =>
                product.Name.ToLower().Contains(searchTerm) ||
                product.Description.ToLower().Contains(searchTerm));
        }

        if (minPrice.HasValue)
            products = products.Where(product => product.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            products = products.Where(product => product.Price <= maxPrice.Value);

        if (isActive.HasValue)
            products = products.Where(product => product.IsActive == isActive.Value);

        return products;
    }

    private static IQueryable<Product> ApplySorting(
        IQueryable<Product> products,
        string sortBy,
        string sortDirection)
    {
        var descending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        return sortBy.ToLowerInvariant() switch
        {
            "createdat" => descending
                ? products.OrderByDescending(product => product.CreatedAt)
                : products.OrderBy(product => product.CreatedAt),
            "name" => descending
                ? products.OrderByDescending(product => product.Name)
                : products.OrderBy(product => product.Name),
            "price" => descending
                ? products.OrderByDescending(product => product.Price)
                : products.OrderBy(product => product.Price),
            _ => products.OrderByDescending(product => product.CreatedAt)
        };
    }
}
