using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Application.UseCases.Category.Shared.Responses;
using Gafel.Application.UseCases.Shared.Responses;
using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Repositories.Category;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.Services.CurrentUser;
using Mapster;

namespace Gafel.Application.UseCases.Category.Filter;

public class FilterCategoryUseCase : IFilterCategoryUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

    public FilterCategoryUseCase(
        ICurrentUser currentUser,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository
        )
    {
        _currentUser = currentUser;
        _personReadOnlyRepository = personReadOnlyRepository;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
    }

    public async Task<PaginationResponse<CategoryResponse>> Execute(FilterCategoryQueryParams query)
    {
        Validate(query);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        bool isPaged = query.Offset.HasValue || query.Limit.HasValue;

        var (categories, total) = await _categoryReadOnlyRepository.Filter(person: person, filters: query);

        return new()
        {
            Items = categories.Adapt<IList<CategoryResponse>>(),
            Offset = isPaged ? query.Offset!.Value : 0,
            Limit = isPaged ? query.Limit!.Value : total,
            Total = total
        };
    }


    private static void Validate(FilterCategoryQueryParams query)
    {
        var result = new FilterCategoryQueryParamsValidator().Validate(query);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
