using Gafel.Application.Exceptions;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Account.GetProfile;

public class GetProfileUseCase : IGetProfileUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;

    public GetProfileUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository)
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
    }


    public async Task<GetProfileResponse> Execute()
    {
        var currentUser = _currentUser.CurrentUser();

        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var response = person.Adapt<GetProfileResponse>();
        response.Email = currentUser.Email;

        return response;
    }
}
