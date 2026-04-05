using Gafel.Application.Services.Mapster;
using System.Runtime.CompilerServices;

namespace UseCases.Test;

public static class Setup
{
    [ModuleInitializer]
    public static void Initialize()
    {
        // Irá executar automaticamente UMA VEZ 
        // assim que o projeto de testes for carregado.
        MapsterConfigurations.Configure();
    }
}
