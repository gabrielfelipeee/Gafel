using Gafel.Domain.Repositories.Person;
using Moq;

namespace CommonTestUtilities.Repositories.Person;

public class PersonReadOnlyRepositoryBuilder
{
    private readonly Mock<IPersonReadOnlyRepository> _repository;
    public PersonReadOnlyRepositoryBuilder() => _repository = new Mock<IPersonReadOnlyRepository>();

    public IPersonReadOnlyRepository Build() => _repository.Object;

    public void GetByUserId(Gafel.Domain.Entities.Person person) 
        => _repository.Setup(repository => repository.GetByUserId(1)).ReturnsAsync(person);

}
