using Gafel.Domain.ValueObjects;

namespace Gafel.Domain.Repositories.Person;

public interface IPersonReadOnlyRepository
{
    Task<Entities.Person?> GetByUserId(Guid userId);
    Task<bool> ExistPersonWithCpf(Cpf cpf, Guid? excludeId = null);
}
