using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Person;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class PersonRepository(GafelDbContext context) : IPersonReadOnlyRepository, IPersonWriteOnlyRepository
{
    private readonly GafelDbContext _context = context;

    public async Task<Person?> GetByUserId(long userId) => await _context.Peoples.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task Add(Person people) => await _context.Peoples.AddAsync(people);
}
