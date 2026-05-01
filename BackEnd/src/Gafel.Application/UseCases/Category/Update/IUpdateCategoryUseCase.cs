using Gafel.Application.UseCases.Category.Shared.Commands;

namespace Gafel.Application.UseCases.Category.Update;

public interface IUpdateCategoryUseCase
{
    Task Execute(long categoryId, CategoryCommand request);
}
