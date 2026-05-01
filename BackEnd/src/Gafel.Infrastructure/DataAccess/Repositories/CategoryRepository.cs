using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Category;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class CategoryRepository(GafelDbContext context) : ICategoryWriteOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Write
    public async Task Add(Category category) => await _context.Categories.AddAsync(category);
}
