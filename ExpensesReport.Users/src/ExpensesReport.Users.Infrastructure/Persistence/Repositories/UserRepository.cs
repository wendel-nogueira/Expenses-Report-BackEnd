using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Repositories;
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

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

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

        public async Task UpdateAsync(Guid id, User userUpdate)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return;

            user.Update(userUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return;

            user.Delete();
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUserSupervisorsByIdAsync(Guid id)
        {
            var supervisors = await _context.Users
                .Include(x => x.Supervisors)
                .ThenInclude(x => x.Supervisor)
                .Where(x => x.Supervisors.Any(x => x.UserId == id))
                .ToListAsync();

            return supervisors;
        }

        public async Task AddSupervisorAsync(Guid userId, Guid supervisorId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return;

            user.AddSupervisorToUser(supervisorId);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupervisorAsync(Guid userId, Guid supervisorId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return;

            user.RemoveSupervisorFromUser(supervisorId);
            await _context.SaveChangesAsync();
        }
    }
}
