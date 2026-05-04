using Gafel.Domain.Dtos.QueryParams;

namespace Gafel.Domain.Repositories.Category;

public interface ICategoryReadOnlyRepository
{
    Task<Entities.Category?> GetById(Entities.Person person, long categoryId);
    Task<(IList<Entities.Category> categories, int total)> Filter(Entities.Person person, FilterCategoryQueryParams filters);
}
