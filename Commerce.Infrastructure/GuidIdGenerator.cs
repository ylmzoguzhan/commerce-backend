using Commerce.Application;

namespace Commerce.Infrastructure;

public sealed class GuidIdGenerator : IIdGenerator
{
    public Guid NewId()
    {
        return Guid.NewGuid();
    }
}
