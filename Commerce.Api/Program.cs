using Commerce.Application.Products;
using Commerce.Application.Products.CreateProduct;
using Commerce.Application.Products.GetProductById;
using Commerce.Infrastructure.Products;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/products", (CreateProductRequest request, CreateProductHandler handler) =>
{
    try
    {
        var command = new CreateProductCommand(request.Name, request.Description, request.Price, request.Currency);
        var result = handler.Handle(command);
        return Results.Created($"/products/{result.Id}", result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/products/{id:guid}", (Guid id, GetProductByIdHandler handler) =>
{
    var query = new GetProductByIdQuery(id);
    var result = handler.Handle(query);

    return result is null
        ? Results.NotFound()
        : Results.Ok(result);
});

app.Run();

record CreateProductRequest(string Name, string Description, decimal Price, string Currency);
