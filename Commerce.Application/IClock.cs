namespace Commerce.Application;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
