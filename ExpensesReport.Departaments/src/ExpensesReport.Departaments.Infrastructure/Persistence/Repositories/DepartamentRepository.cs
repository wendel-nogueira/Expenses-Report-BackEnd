using ExpensesReport.Departaments.Core.Entities;
using ExpensesReport.Departaments.Core.Repositories;
using ExpensesReport.Departaments.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Departaments.Infrastructure.Persistence.Repositories
{
    public class DepartamentRepository : IDepartamentRepository
    {
        private readonly DepartamentsDbContext _context;

        public DepartamentRepository(DepartamentsDbContext context)
        {
            _context = context;
        }

        public async Task<Departament?> GetByIdAsync(Guid id)
        {
            return await _context.Departaments.FirstOrDefaultAsync(departament => departament.Id == id);
        }

        public async Task<Departament?> GetByNameAsync(string name)
        {
            return await _context.Departaments.FirstOrDefaultAsync(departament => departament.Name == name);
        }

        public async Task<Departament?> GetByAcronymAsync(string acronym)
        {
            return await _context.Departaments.FirstOrDefaultAsync(departament => departament.Acronym == acronym);
        }

        public async Task<IEnumerable<Departament>> GetAllAsync()
        {
            return await _context.Departaments.ToListAsync();
        }

        public async Task AddAsync(Departament departament)
        {
            _context.Departaments.Add(departament);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Departament departament)
        {
            _context.Departaments.Update(departament);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Departament departament)
        {
            departament.Delete();
            await _context.SaveChangesAsync();
        }

        public async Task ActivateAsync(Departament departament)
        {
            departament.Activate();
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Manager>> GetAllManagersAsync(Guid id)
        {
            var departament = await _context.Departaments
                .Include(departament => departament.Managers)
                .FirstOrDefaultAsync(departament => departament.Id == id);

            var managers = await _context.Entry(departament!)
                .Collection(departament => departament.Managers)
                .Query()
                .ToListAsync();

            return managers;
        }

        public async Task AddManagerAsync(Guid departamentId, Guid managerId)
        {
            var departament = await _context.Departaments
                .Include(departament => departament.Managers)
                .FirstOrDefaultAsync(departament => departament.Id == departamentId);
            var manager = new Manager(managerId, departament!);

            _context.Managers.Add(manager);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveManagerAsync(Guid departamentId, Guid managerId)
        {
            var manager = await _context.Managers
                .FirstOrDefaultAsync(manager => manager.ManagerId == managerId && manager.DepartamentId == departamentId);

            _context.Managers.Remove(manager!);

            _context.SaveChanges();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(Guid id)
        {
            var departament = await _context.Departaments
                .Include(departament => departament.Users)
                .FirstOrDefaultAsync(departament => departament.Id == id);
            var users = await _context.Entry(departament!)
                .Collection(departament => departament.Users)
                .Query()
                .ToListAsync();

            return users;
        }

        public async Task AddUserAsync(Guid departamentId, Guid userId)
        {
            var departament = await _context.Departaments
                .Include(departament => departament.Users)
                .FirstOrDefaultAsync(departament => departament.Id == departamentId);
            var user = new User(userId, departament!);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(Guid departamentId, Guid userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(manager => manager.UserId == userId && manager.DepartamentId == departamentId);

            _context.Users.Remove(user!);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Departament>> GetDepartamentsByManagerAndUser(Guid id)
        {
            var departamentsByMager = await _context.Departaments
                .Include(departament => departament.Managers)
                .Where(departament => departament.Managers.Any(manager => manager.ManagerId == id))
                .ToListAsync();

            var departamentsByUser = await _context.Departaments
                .Include(departament => departament.Users)
                .Where(departament => departament.Users.Any(user => user.UserId == id))
                .ToListAsync();

            return departamentsByMager.Concat(departamentsByUser).Distinct();
        }
    }
}
