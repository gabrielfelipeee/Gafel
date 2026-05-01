using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Category.Shared.Responses;
using Gafel.Domain.Repositories.Category;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Category.GetById;

public class GetCategoryByIdUseCase : IGetCategoryByIdUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

    public GetCategoryByIdUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository
        )
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
    }

    public async Task<CategoryResponse> Execute(long categoryId)
    {
        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        var category = await _categoryReadOnlyRepository.GetById(person: person, categoryId: categoryId)
            ?? throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

        var response = category.Adapt<CategoryResponse>();

        return response;
    }
}
