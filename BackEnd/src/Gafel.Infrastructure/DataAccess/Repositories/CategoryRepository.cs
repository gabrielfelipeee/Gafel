using Gafel.Domain.Dtos.QueryParams;
using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Category;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class CategoryRepository(GafelDbContext context) : ICategoryReadOnlyRepository, ICategoryWriteOnlyRepository, ICategoryUpdateOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Read
    async Task<Category?> ICategoryReadOnlyRepository.GetById(Person person, long categoryId)
        => await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.IsActive && category.PersonId == person.Id && category.Id == categoryId);

    public async Task<(IList<Category> categories, int total)> Filter(Person person, FilterCategoryQueryParams filters)
    {
        IQueryable<Category> query = _context.Categories
            .AsNoTracking()
            .Where(category => category.IsActive && category.PersonId == person.Id)
            .OrderBy(x => x.Id);

        if (filters.Type.HasValue)
            query = query.Where(category => category.Type == filters.Type.Value);

        int total = await query.CountAsync();

        if (filters.Offset.HasValue && filters.Limit.HasValue)
        {
            query = query
                .Skip(filters.Offset.Value)
                .Take(filters.Limit.Value);
        }

        return (await query.ToListAsync(), total);
    }

    // Write
    public async Task Add(Category category) => await _context.Categories.AddAsync(category);


    // Update
    async Task<Category?> ICategoryUpdateOnlyRepository.GetById(Person person, long categoryId)
        => await _context.Categories.FirstOrDefaultAsync(category => category.IsActive && category.PersonId == person.Id && category.Id == categoryId);

    public void Update(Category category) => _context.Categories.Update(category);
}
