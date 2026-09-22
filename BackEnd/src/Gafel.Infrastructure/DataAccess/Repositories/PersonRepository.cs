using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Person;
using Gafel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class PersonRepository(GafelDbContext context) : IPersonReadOnlyRepository, IPersonWriteOnlyRepository, IPersonUpdateOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Read
    async Task<Person?> IPersonReadOnlyRepository.GetByUserId(Guid userId)
        => await _context.People.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task<bool> ExistPersonWithCpf(Cpf cpf, Guid? excludeId = null)
    {
        return await _context.People
            .AsNoTracking()
            .AnyAsync(p =>
                p.Cpf != null &&
                p.Cpf == cpf &&
                (!excludeId.HasValue || p.Id != excludeId.Value));
    }

    // Write
    public async Task Add(Person person) => await _context.People.AddAsync(person);


    // Update
    async Task<Person?> IPersonUpdateOnlyRepository.GetByUserId(Guid userId)
        => await _context.People.FirstOrDefaultAsync(p => p.UserId == userId);

    public void Update(Person person) => _context.People.Update(person);
}
