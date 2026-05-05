using Gafel.Domain.Dtos.QueryParams;
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

    public ICategoryReadOnlyRepository Filter(Gafel.Domain.Entities.Person person, IList<Gafel.Domain.Entities.Category> categories)
    {
        _repository
            .Setup(repository => repository.Filter(person, It.IsAny<FilterCategoryQueryParams>()))
            .ReturnsAsync((categories, categories.Count));

        return _repository.Object;
    }

    public void ExistActiveCategoryWithId(Gafel.Domain.Entities.Person person, long categoryId)
    {
        _repository
            .Setup(repository => repository.ExistActiveCategoryWithId(person, categoryId))
            .ReturnsAsync(true);
    }
}
