using Gafel.Application.UseCases.Category.Shared.Commands;

namespace Gafel.Application.UseCases.Category.Register;

public interface IRegisterCategoryUseCase
{
    Task Execute(CategoryCommand request);
}
