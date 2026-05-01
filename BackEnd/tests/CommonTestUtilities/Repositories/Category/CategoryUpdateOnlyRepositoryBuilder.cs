using Gafel.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories.Category;

public class CategoryUpdateOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryUpdateOnlyRepository> _repository;
    public CategoryUpdateOnlyRepositoryBuilder() => _repository = new Mock<ICategoryUpdateOnlyRepository>();

    public ICategoryUpdateOnlyRepository Build() => _repository.Object;

    public void GetById(Gafel.Domain.Entities.Person person, Gafel.Domain.Entities.Category category)
        => _repository.Setup(repository => repository.GetById(person, category.Id)).ReturnsAsync(category);
}
