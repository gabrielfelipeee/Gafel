namespace Gafel.Domain.Repositories.Person;

public interface IPersonWriteOnlyRepository
{
    Task Add(Entities.Person person);
}
