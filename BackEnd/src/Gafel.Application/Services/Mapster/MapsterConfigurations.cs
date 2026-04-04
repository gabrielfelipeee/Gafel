using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Domain.Entities;
using Mapster;

namespace Gafel.Application.Services.Mapster;

public static class MapsterConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<Person, GetProfileResponse>
            .NewConfig()
            .Map(dest => dest.Cpf, src => src.Cpf == null ? null : src.Cpf.Value)
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth == null ? (DateOnly?)null : src.DateOfBirth.Value)
            .Ignore(dest => dest.Email);
    }
}
