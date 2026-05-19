namespace Commerce.Application.Products.ChangeProductName;

public record ChangeProductNameCommand(Guid Id, string NewName);
