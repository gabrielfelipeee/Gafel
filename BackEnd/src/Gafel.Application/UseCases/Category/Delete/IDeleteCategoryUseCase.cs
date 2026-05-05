namespace Gafel.Application.UseCases.Category.Delete;

public interface IDeleteCategoryUseCase
{
    Task Execute(long categoryId);
}
