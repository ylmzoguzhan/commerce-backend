using Commerce.Application.Products.GetProducts;
using Commerce.Domain;
using Xunit;

namespace Commerce.Tests;

public class GetProductsHandlerTests
{
    [Fact]
    public void Handle_WhenRepositoryIsEmpty_ShouldReturnEmptyPagedResponse()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);

        var result = handler.Handle(new GetProductsQuery(Page: 1, PageSize: 20));

        Assert.Empty(result.Items);
        Assert.Equal(1, result.Page);
        Assert.Equal(20, result.PageSize);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.TotalPages);
    }

    [Fact]
    public void Handle_WhenProductsExist_ShouldReturnTotalCount()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var firstProduct = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        var secondProduct = Product.Create("Phone", "Smart phone", 30000, "TRY");
        repository.Add(firstProduct);
        repository.Add(secondProduct);

        var result = handler.Handle(new GetProductsQuery(Page: 1, PageSize: 10));

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
    }

    [Fact]
    public void Handle_WithPageSizeOne_ShouldReturnOneItem()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        repository.Add(Product.Create("Laptop", "Gaming laptop", 45000, "TRY"));
        repository.Add(Product.Create("Phone", "Smart phone", 30000, "TRY"));

        var result = handler.Handle(new GetProductsQuery(Page: 1, PageSize: 1));

        Assert.Single(result.Items);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
    }

    [Fact]
    public void Handle_WithSecondPage_ShouldSkipPreviousItems()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var firstProduct = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        var secondProduct = Product.Create("Phone", "Smart phone", 30000, "TRY");
        repository.Add(firstProduct);
        repository.Add(secondProduct);

        var result = Assert.Single(handler.Handle(new GetProductsQuery(Page: 2, PageSize: 1)).Items);

        Assert.Equal(secondProduct.Id, result.Id);
    }

    [Fact]
    public void Handle_WithDefaultSorting_ShouldReturnNewestProductsFirst()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var olderProduct = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        Thread.Sleep(1);
        var newerProduct = Product.Create("Phone", "Smart phone", 30000, "TRY");
        repository.Add(olderProduct);
        repository.Add(newerProduct);

        var result = handler.Handle(new GetProductsQuery(Page: 1, PageSize: 20));

        Assert.Equal(newerProduct.Id, result.Items.First().Id);
        Assert.Equal(olderProduct.Id, result.Items.Last().Id);
    }

    [Fact]
    public void Handle_WithNameAscending_ShouldSortByName()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var secondProduct = Product.Create("Phone", "Smart phone", 30000, "TRY");
        var firstProduct = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(secondProduct);
        repository.Add(firstProduct);

        var result = handler.Handle(new GetProductsQuery(
            Page: 1,
            PageSize: 20,
            SortBy: "name",
            SortDirection: "asc"));

        Assert.Equal(firstProduct.Id, result.Items.First().Id);
        Assert.Equal(secondProduct.Id, result.Items.Last().Id);
    }

    [Fact]
    public void Handle_WithPriceDescending_ShouldSortByPrice()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var cheaperProduct = Product.Create("Phone", "Smart phone", 30000, "TRY");
        var expensiveProduct = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(cheaperProduct);
        repository.Add(expensiveProduct);

        var result = handler.Handle(new GetProductsQuery(
            Page: 1,
            PageSize: 20,
            SortBy: "price",
            SortDirection: "desc"));

        Assert.Equal(expensiveProduct.Id, result.Items.First().Id);
        Assert.Equal(cheaperProduct.Id, result.Items.Last().Id);
    }

    [Fact]
    public void Handle_ShouldApplySortingBeforePagination()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var cheapestProduct = Product.Create("Mouse", "Wireless mouse", 1000, "TRY");
        var middleProduct = Product.Create("Phone", "Smart phone", 30000, "TRY");
        var expensiveProduct = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(cheapestProduct);
        repository.Add(middleProduct);
        repository.Add(expensiveProduct);

        var result = Assert.Single(handler.Handle(new GetProductsQuery(
            Page: 2,
            PageSize: 1,
            SortBy: "price",
            SortDirection: "desc")).Items);

        Assert.Equal(middleProduct.Id, result.Id);
    }

    [Fact]
    public void Handle_WhenProductsExist_ShouldMapProductFields()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);
        var product = Product.Create("Laptop", "Gaming laptop", 45000, "TRY");
        repository.Add(product);

        var result = Assert.Single(handler.Handle(new GetProductsQuery(Page: 1, PageSize: 20)).Items);

        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Description, result.Description);
        Assert.Equal(product.Price, result.Price);
        Assert.Equal(product.Currency, result.Currency);
        Assert.Equal(product.IsActive, result.IsActive);
        Assert.Equal(product.CreatedAt, result.CreatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Handle_WithInvalidPage_ShouldThrowException(int page)
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            handler.Handle(new GetProductsQuery(Page: page, PageSize: 20)));

        Assert.Equal("Page", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Handle_WithInvalidPageSize_ShouldThrowException(int pageSize)
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            handler.Handle(new GetProductsQuery(Page: 1, PageSize: pageSize)));

        Assert.Equal("PageSize", exception.ParamName);
    }

    [Fact]
    public void Handle_WithTooLargePageSize_ShouldThrowException()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            handler.Handle(new GetProductsQuery(Page: 1, PageSize: 101)));

        Assert.Equal("PageSize", exception.ParamName);
    }

    [Fact]
    public void Handle_WithInvalidSortBy_ShouldThrowException()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);

        var exception = Assert.Throws<ArgumentException>(() =>
            handler.Handle(new GetProductsQuery(
                Page: 1,
                PageSize: 20,
                SortBy: "unknown",
                SortDirection: "desc")));

        Assert.Equal("SortBy", exception.ParamName);
    }

    [Fact]
    public void Handle_WithInvalidSortDirection_ShouldThrowException()
    {
        var repository = new FakeProductRepository();
        var handler = new GetProductsHandler(repository);

        var exception = Assert.Throws<ArgumentException>(() =>
            handler.Handle(new GetProductsQuery(
                Page: 1,
                PageSize: 20,
                SortBy: "createdAt",
                SortDirection: "down")));

        Assert.Equal("SortDirection", exception.ParamName);
    }
}
