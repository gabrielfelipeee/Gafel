using Gafel.Domain.Entities;
using Gafel.Domain.Repositories.Person;
using Microsoft.EntityFrameworkCore;

namespace Gafel.Infrastructure.DataAccess.Repositories;

public class PersonRepository(GafelDbContext context) : IPersonReadOnlyRepository, IPersonWriteOnlyRepository, IPersonUpdateOnlyRepository
{
    private readonly GafelDbContext _context = context;

    // Read
    async Task<Person?> IPersonReadOnlyRepository.GetByUserId(long userId)
        => await _context.People.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task<bool> ExistPersonWithCpf(string cpf, long? excludeId = null)
        => await _context.People.AsNoTracking().AnyAsync(p => p.Cpf == cpf && (!excludeId.HasValue || p.Id != excludeId));



    // Write
    public async Task Add(Person people) => await _context.People.AddAsync(people);


    // Update
    async Task<Person?> IPersonUpdateOnlyRepository.GetByUserId(long userId)
        => await _context.People.FirstOrDefaultAsync(p => p.UserId == userId);

    public void Update(Person person) => _context.People.Update(person);
}
