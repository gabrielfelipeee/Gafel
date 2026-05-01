using Gafel.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories.Category;

public class CategoryReadOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryReadOnlyRepository> _repository;
    public CategoryReadOnlyRepositoryBuilder() => _repository = new Mock<ICategoryReadOnlyRepository>();

    public ICategoryReadOnlyRepository Build() => _repository.Object;

    public void GetById(Gafel.Domain.Entities.Person person, Gafel.Domain.Entities.Category category)
        => _repository.Setup(repository => repository.GetById(person, category.Id)).ReturnsAsync(category);
}
