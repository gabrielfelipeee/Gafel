namespace Gafel.Domain.Repositories.Category;

public interface ICategoryUpdateOnlyRepository
{
    Task<Entities.Category?> GetById(Entities.Person person, long categoryId);
    void Update(Entities.Category category);
}
