namespace Commerce.Api.Endpoints.Requests;

public sealed record CreateProductRequest(string Name, string Description, decimal Price, string Currency);
