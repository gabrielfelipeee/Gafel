using Gafel.Domain.Dtos.QueryParams;

namespace Gafel.Domain.Repositories.Category;

public interface ICategoryReadOnlyRepository
{
    Task<Entities.Category?> GetById(Entities.Person person, Guid categoryId);
    Task<(IList<Entities.Category> categories, int total)> Filter(Entities.Person person, FilterCategoryQueryParams filters);
    Task<bool> ExistActiveCategoryWithId(Entities.Person person, Guid categoryId);
}
