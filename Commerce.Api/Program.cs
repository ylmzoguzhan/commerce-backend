using Commerce.Application.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<CreateProductHandler>();

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

app.Run();

record CreateProductRequest(string Name, string Description, decimal Price, string Currency);
