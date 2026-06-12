using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Application.UseCases.Category.Shared.Responses;
using Gafel.Domain.Entities;
using Mapster;
using Sqids;

namespace Gafel.Application.Services.Mapster;

public static class MapsterConfigurations
{
    public static void Configure(TypeAdapterConfig config, SqidsEncoder<long> sqids)
    {
        DomainToResponse(config, sqids);
    }

    private static void DomainToResponse(TypeAdapterConfig config, SqidsEncoder<long> sqids)
    {
        config.NewConfig<Person, GetProfileResponse>()
            .Map(dest => dest.Cpf, src => src.Cpf == null ? null : src.Cpf.Value)
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth == null ? (DateOnly?)null : src.DateOfBirth.Value)
            .Map(dest => dest.Uf, src => src.Uf == null ? null : src.Uf.ToString())
            .Ignore(dest => dest.Email);


        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.Id, src => sqids.Encode(src.Id));
    }
}
