using Commerce.Application;

namespace Commerce.Infrastructure;

public class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
