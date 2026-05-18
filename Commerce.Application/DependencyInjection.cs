using Commerce.Application.Products.CreateProduct;
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
        return services;
    }
}
