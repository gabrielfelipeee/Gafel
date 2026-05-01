using Gafel.Application.UseCases.Category.Shared.Responses;

namespace Gafel.Application.UseCases.Category.GetById;

public interface IGetCategoryByIdUseCase
{
    Task<CategoryResponse> Execute(long categoryId);
}
