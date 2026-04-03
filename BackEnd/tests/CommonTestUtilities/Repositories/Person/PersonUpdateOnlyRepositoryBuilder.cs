using Gafel.Domain.Repositories.Person;
using Moq;

namespace CommonTestUtilities.Repositories.Person;

public class PersonUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IPersonUpdateOnlyRepository> _repository;
    public PersonUpdateOnlyRepositoryBuilder() => _repository = new Mock<IPersonUpdateOnlyRepository>();

    public IPersonUpdateOnlyRepository Build() => _repository.Object;

    public void GetByUserId(Gafel.Domain.Entities.Person person) 
        => _repository.Setup(repository => repository.GetByUserId(1)).ReturnsAsync(person);
}
