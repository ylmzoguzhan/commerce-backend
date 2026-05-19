using Commerce.Domain;

namespace Commerce.Application.Products;

public interface IProductRepository
{
    void Add(Product product);
    Product? GetById(Guid id);
    IReadOnlyCollection<Product> GetPaged(int page, int pageSize);
    int Count();
}
