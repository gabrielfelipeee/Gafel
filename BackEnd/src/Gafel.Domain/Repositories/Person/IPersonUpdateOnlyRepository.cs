namespace Gafel.Domain.Repositories.Person;

public interface IPersonUpdateOnlyRepository
{
    Task<Entities.Person?> GetByUserId(long userId);
    void Update(Entities.Person person);
}
