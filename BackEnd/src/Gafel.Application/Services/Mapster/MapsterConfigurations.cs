using Gafel.Application.UseCases.Person.Update;
using Gafel.Domain.Entities;
using Gafel.Domain.Enums;
using Mapster;

namespace Gafel.Application.Services.Mapster;

public static class MapsterConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<UpdatePersonCommand, Person>
            .NewConfig()
            .Map(dest => dest.Uf, src => string.IsNullOrWhiteSpace(src.Uf) ? (Uf?)null : Enum.Parse<Uf>(value: src.Uf, ignoreCase: true));
    }
}
