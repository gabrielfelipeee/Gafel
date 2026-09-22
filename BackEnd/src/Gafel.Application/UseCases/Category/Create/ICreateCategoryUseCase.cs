using Gafel.Application.UseCases.Category.Shared.Commands;

namespace Gafel.Application.UseCases.Category.Create;

public interface ICreateCategoryUseCase
{
    Task Execute(CategoryCommand request);
}
