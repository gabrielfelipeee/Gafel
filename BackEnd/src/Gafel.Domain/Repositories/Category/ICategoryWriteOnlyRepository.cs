namespace Gafel.Domain.Repositories.Category;

public interface ICategoryWriteOnlyRepository
{
    Task Add(Entities.Category category);
    Task Delete(long categoryId);
}
