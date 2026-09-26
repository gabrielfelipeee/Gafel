using Bogus;

namespace CommonTestUtilities;

public static class FakerFactory
{
    public static Faker<T> Create<T>() where T : class => new("pt_BR");
}
