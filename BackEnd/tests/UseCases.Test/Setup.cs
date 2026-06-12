using Gafel.Application.Services.Mapster;
using Mapster;
using System.Runtime.CompilerServices;
using CommonTestUtilities.IdObfuscation;

namespace UseCases.Test;

public static class Setup
{
    [ModuleInitializer]
    public static void Initialize()
    {
        // Irá executar automaticamente UMA VEZ 
        // assim que o projeto de testes for carregado.

        var sqids = IdObfuscationBuilder.Build();
        var config = TypeAdapterConfig.GlobalSettings;

        MapsterConfigurations.Configure(config, sqids);
    }
}
