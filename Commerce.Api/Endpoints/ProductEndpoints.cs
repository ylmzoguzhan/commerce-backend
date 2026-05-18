using Commerce.Application.Products.CreateProduct;
using Commerce.Application.Products.GetProductById;
using Commerce.Application.Products.GetProducts;

namespace Commerce.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/products", (CreateProductRequest request, CreateProductHandler handler) =>
        {
            var command = new CreateProductCommand(request.Name, request.Description, request.Price, request.Currency);
            var result = handler.Handle(command);
            return Results.Created($"/products/{result.Id}", result);
        });

        app.MapGet("/products/{id:guid}", (Guid id, GetProductByIdHandler handler) =>
        {
            var query = new GetProductByIdQuery(id);
            var result = handler.Handle(query);

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });
        app.MapGet("/products", (GetProductsHandler handler) =>
        {
            var result = handler.Handle();

            return Results.Ok(result);
        });

        return app;
    }
}

public sealed record CreateProductRequest(string Name, string Description, decimal Price, string Currency);
