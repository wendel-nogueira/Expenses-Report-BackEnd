using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Repositories;
using ExpensesReport.Users.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Users.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;

        public UserRepository(UsersDbContext usersDbContext)
        {
            _context = usersDbContext;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<User?> GetByIdentityIdAsync(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.IdentityId == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, UserName name, UserAddress address)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return;

            user.Update(name, address);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUserSupervisorsByIdAsync(Guid id)
        {
            IEnumerable<User> supervisors = new List<User>();

            var user = await _context.Users.Include(x => x.Supervisors).SingleOrDefaultAsync(x => x.Id == id);

            if (user == null || user.Supervisors == null || user.Supervisors.Count == 0)
                return supervisors;

            var supervisorsIds = user.Supervisors.Select(x => x.SupervisorId).ToList();

            if (supervisorsIds.Count == 0)
                return supervisors;

            supervisors = await _context.Users.Where(x => supervisorsIds.Contains(x.Id)).ToListAsync();

            return supervisors;
        }

        public async Task AddSupervisorAsync(Guid userId, Guid supervisorId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return;

            var supervisor = await _context.Users.SingleOrDefaultAsync(x => x.Id == supervisorId);

            if (supervisor == null)
                return;

            user.AddSupervisorToUser(supervisor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupervisorAsync(Guid userId, Guid supervisorId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return;

            var supervisor = await _context.Users.SingleOrDefaultAsync(x => x.Id == supervisorId);

            if (supervisor == null)
                return;

            user.RemoveSupervisorFromUser(supervisor);
            await _context.SaveChangesAsync();
        }
    }
}
