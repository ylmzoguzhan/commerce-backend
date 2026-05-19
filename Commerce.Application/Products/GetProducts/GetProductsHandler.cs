namespace Commerce.Application.Products.GetProducts;

public sealed class GetProductsHandler(IProductRepository repository)
{
    private const int MaxPageSize = 100;
    private static readonly string[] ValidSortFields = ["createdAt", "name", "price"];
    private static readonly string[] ValidSortDirections = ["asc", "desc"];

    public GetProductsResponse Handle(GetProductsQuery query)
    {
        if (query.Page < 1)
            throw new ArgumentOutOfRangeException(nameof(query.Page), "Page 1 veya daha buyuk olmalidir");

        if (query.PageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(query.PageSize), "PageSize 1 veya daha buyuk olmalidir");

        if (query.PageSize > MaxPageSize)
            throw new ArgumentOutOfRangeException(nameof(query.PageSize), $"PageSize {MaxPageSize} veya daha kucuk olmalidir");

        if (!ValidSortFields.Contains(query.SortBy, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Gecersiz sort alani", nameof(query.SortBy));

        if (!ValidSortDirections.Contains(query.SortDirection, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Gecersiz sort yonu", nameof(query.SortDirection));

        var products = repository
            .GetPaged(query.Page, query.PageSize, query.SortBy, query.SortDirection)
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
