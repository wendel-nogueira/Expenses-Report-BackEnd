using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Repositories;

namespace ExpensesReport.Users.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _usersDbContext;

        public UserRepository(UsersDbContext usersDbContext)
        {
            _usersDbContext = usersDbContext;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user = _usersDbContext.Users.SingleOrDefault(x => x.Id == id);

            await Task.CompletedTask;

            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = _usersDbContext.Users.SingleOrDefault(x => x.Email == email);

            await Task.CompletedTask;

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = _usersDbContext.Users.Where(user => !user.IsDeleted).ToList();

            await Task.CompletedTask;

            return users;
        }

        public async Task AddAsync(User user)
        {
            _usersDbContext.Users.Add(user);

            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Guid id, User userUpdate)
        {
            var user = _usersDbContext.Users.SingleOrDefault(x => x.Id == id);

            await Task.CompletedTask;

            if (user == null)
            {
                return;
            }

            user.Update(userUpdate);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = _usersDbContext.Users.SingleOrDefault(x => x.Id == id);

            await Task.CompletedTask;


            if (user == null)
            {
                return;
            }

            user.Delete();
        }

        public async Task AddSupervisorAsync(Guid userId, Guid supervisorId)
        {
            var user = _usersDbContext.Users.SingleOrDefault(x => x.Id == userId);

            await Task.CompletedTask;

            if (user == null)
            {
                return;
            }

            user.AddSupervisorToUser(supervisorId);
        }

        public async Task DeleteSupervisorAsync(Guid userId, Guid supervisorId)
        {
            var user = _usersDbContext.Users.SingleOrDefault(x => x.Id == userId);

            await Task.CompletedTask;

            if (user == null)
            {
                return;
            }

            user.RemoveSupervisorFromUser(supervisorId);
        }
    }
}
