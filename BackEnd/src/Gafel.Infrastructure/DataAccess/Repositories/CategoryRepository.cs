using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Category;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class CategoryRepository(GafelDbContext context) : ICategoryWriteOnlyRepository, ICategoryUpdateOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Write
    public async Task Add(Category category) => await _context.Categories.AddAsync(category);


    // Update
    public async Task<Category?> GetById(Person person, long categoryId)
        => await _context.Categories.FirstOrDefaultAsync(category => category.IsActive && category.PersonId == person.Id && category.Id == categoryId);

    public void Update(Category category) => _context.Categories.Update(category);
}
