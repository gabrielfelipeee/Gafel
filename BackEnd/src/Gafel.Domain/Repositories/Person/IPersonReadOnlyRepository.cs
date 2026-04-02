using Gafel.Domain.ValueObjects;

namespace Gafel.Domain.Repositories.Person;

public interface IPersonReadOnlyRepository
{
    Task<Entities.Person?> GetByUserId(long userId);
    Task<bool> ExistPersonWithCpf(Cpf cpf, long? excludeId = null);
}
