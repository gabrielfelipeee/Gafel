namespace Gafel.Domain.Repositories.Person;

public interface IPersonUpdateOnlyRepository
{
    Task<Entities.Person?> GetByUserId(Guid userId);
    void Update(Entities.Person person);
}
