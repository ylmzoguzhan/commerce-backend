using Commerce.Domain;

namespace Commerce.Application.Products;

public interface IProductRepository
{
    void Add(Product product);
    Product? GetById(Guid id);
    IReadOnlyCollection<Product> GetPaged(
        int page,
        int pageSize,
        string sortBy,
        string sortDirection,
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        bool? isActive);

    int Count(
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        bool? isActive);
}
