using Gafel.Application.UseCases.Shared.Responses;

namespace Gafel.Application.UseCases.Category.GetById;

public interface IGetCategoryByIdUseCase
{
    Task<CategoryResponse> Execute(Guid categoryId);
}
