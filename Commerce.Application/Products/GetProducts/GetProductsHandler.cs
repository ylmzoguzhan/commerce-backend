namespace Commerce.Application.Products.GetProducts;

public sealed class GetProductsHandler(IProductRepository repository)
{
    private const int MaxPageSize = 100;

    public GetProductsResponse Handle(GetProductsQuery query)
    {
        if (query.Page < 1)
            throw new ArgumentOutOfRangeException(nameof(query.Page), "Page 1 veya daha buyuk olmalidir");

        if (query.PageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(query.PageSize), "PageSize 1 veya daha buyuk olmalidir");

        if (query.PageSize > MaxPageSize)
            throw new ArgumentOutOfRangeException(nameof(query.PageSize), $"PageSize {MaxPageSize} veya daha kucuk olmalidir");

        var products = repository
            .GetPaged(query.Page, query.PageSize)
            .Select(product => new GetProductsResult(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Currency,
                product.IsActive,
                product.CreatedAt)).ToList();
        var totalCount = repository.Count();
        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new GetProductsResponse(products, query.Page, query.PageSize, totalCount, totalPages);
    }
}
