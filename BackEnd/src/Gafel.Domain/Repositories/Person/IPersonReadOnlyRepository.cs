namespace Gafel.Domain.Repositories.Person;

public interface IPersonReadOnlyRepository
{
    Task<Entities.Person?> GetByUserId(long userId);
}
