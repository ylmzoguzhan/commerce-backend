using Commerce.Api.Endpoints.Requests;
using Commerce.Application.Products.ActivateProduct;
using Commerce.Application.Products.ChangeProductDescription;
using Commerce.Application.Products.ChangeProductName;
using Commerce.Application.Products.ChangeProductPrice;
using Commerce.Application.Products.CreateProduct;
using Commerce.Application.Products.DeactivateProduct;
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
        app.MapGet("/products", (GetProductsHandler handler, string? search = null, decimal? minPrice = null, decimal? maxPrice = null, bool? isActive = null, int page = 1, int pageSize = 20, string sortBy = "createdAt", string sortDirection = "desc") =>
        {
            var query = new GetProductsQuery(page, pageSize, sortBy, sortDirection, search, minPrice, maxPrice, isActive);
            var result = handler.Handle(query);

            return Results.Ok(result);
        });

        app.MapPatch("/products/{id:guid}/price", (Guid id, ChangeProductPriceRequest request, ChangeProductPriceHandler handler) =>
        {
            var result = handler.Handle(new ChangeProductPriceCommand(id, request.NewPrice));

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });
        app.MapPatch("/products/{id:guid}/deactivate", (Guid id, DeactivateProductHandler handler) =>
        {
            var result = handler.Handle(new DeactivateProductCommand(id));

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });
        app.MapPatch("/products/{id:guid}/activate", (Guid id, ActivateProductHandler handler) =>
        {
            var result = handler.Handle(new ActivateProductCommand(id));

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });
        app.MapPatch("/products/{id:guid}/name", (Guid id, ChangeProductNameRequest request, ChangeProductNameHandler handler) =>
        {
            var result = handler.Handle(new ChangeProductNameCommand(id, request.Name));

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });
        app.MapPatch("/products/{id:guid}/description", (Guid id, ChangeProductDescriptionRequest request, ChangeProductDescriptionHandler handler) =>
        {
            var result = handler.Handle(new ChangeProductDescriptionCommand(id, request.Description));

            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        });
        return app;
    }
}

