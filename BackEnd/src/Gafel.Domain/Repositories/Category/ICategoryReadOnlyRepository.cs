namespace Gafel.Domain.Repositories.Category;

public interface ICategoryReadOnlyRepository
{
    Task<Entities.Category?> GetById(Entities.Person person, long categoryId);
}
