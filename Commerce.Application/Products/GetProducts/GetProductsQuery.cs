namespace Commerce.Application.Products.GetProducts;

public sealed record GetProductsQuery(
    int Page,
    int PageSize,
    string SortBy = "createdAt",
    string SortDirection = "desc");
