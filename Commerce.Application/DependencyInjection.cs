using Commerce.Application.Products.ActivateProduct;
using Commerce.Application.Products.ChangeProductDescription;
using Commerce.Application.Products.ChangeProductName;
using Commerce.Application.Products.ChangeProductPrice;
using Commerce.Application.Products.CreateProduct;
using Commerce.Application.Products.DeactivateProduct;
using Commerce.Application.Products.GetProductById;
using Commerce.Application.Products.GetProducts;
using Microsoft.Extensions.DependencyInjection;

namespace Commerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<GetProductsHandler>();
        services.AddScoped<ChangeProductPriceHandler>();
        services.AddScoped<DeactivateProductHandler>();
        services.AddScoped<ActivateProductHandler>();
        services.AddScoped<ChangeProductNameHandler>();
        services.AddScoped<ChangeProductDescriptionHandler>();
        return services;
    }
}
