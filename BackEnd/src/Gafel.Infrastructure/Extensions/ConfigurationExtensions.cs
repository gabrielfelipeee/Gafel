using Microsoft.Extensions.Configuration;

namespace Gafel.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("ConnectionString")!;
    }
}
