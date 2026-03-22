using Microsoft.Extensions.Configuration;

namespace Gafel.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("ConnectionString")!;

    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
        => configuration.GetValue<bool>("InMemoryTest");
}
