using Gafel.Domain.Repositories.Person;
using Gafel.Domain.ValueObjects;
using Moq;

namespace CommonTestUtilities.Repositories.Person;

public class PersonReadOnlyRepositoryBuilder
{
    private readonly Mock<IPersonReadOnlyRepository> _repository;
    public PersonReadOnlyRepositoryBuilder() => _repository = new Mock<IPersonReadOnlyRepository>();

    public IPersonReadOnlyRepository Build() => _repository.Object;

    public void GetByUserId(Gafel.Domain.Entities.Person person)
        => _repository.Setup(repository => repository.GetByUserId(1)).ReturnsAsync(person);

    public void ExistPersonWithCpf() => _repository.Setup(r => r.ExistPersonWithCpf(It.IsAny<Cpf>(), It.IsAny<long>())).ReturnsAsync(true);
}
