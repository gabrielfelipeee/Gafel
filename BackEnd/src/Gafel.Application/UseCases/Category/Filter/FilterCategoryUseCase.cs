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

    public async Task<PaginationResponse<CategoryResponse>> Execute(FilterCategoryQueryParams queryParams)
    {
        Validate(queryParams);

        var currentUser = _currentUser.CurrentUser();
        var person = await _personReadOnlyRepository.GetByUserId(currentUser.Id) ?? throw new PersonNotFoundException();

        bool isPaged = queryParams.Offset.HasValue || queryParams.Limit.HasValue;

        var (categories, total) = await _categoryReadOnlyRepository.Filter(person: person, filters: queryParams);

        return new()
        {
            Items = categories.Adapt<IList<CategoryResponse>>(),
            Offset = isPaged ? queryParams.Offset!.Value : 0,
            Limit = isPaged ? queryParams.Limit!.Value : total,
            Total = total
        };
    }


    private static void Validate(FilterCategoryQueryParams queryParams)
    {
        var result = new FilterCategoryQueryParamsValidator().Validate(queryParams);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
