using Gafel.Application.UseCases.Category.Shared.Responses;
using Gafel.Application.UseCases.Shared.Responses;
using Gafel.Domain.Dtos.QueryParams;

namespace Gafel.Application.UseCases.Category.Filter;

public interface IFilterCategoryUseCase
{
    Task<PaginationResponse<CategoryResponse>> Execute(FilterCategoryQueryParams query);
}
