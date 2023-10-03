using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.ValueObjects;

namespace ExpensesReport.Users.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByIdentityIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(Guid id, UserName name, UserAddress address);
        Task<IEnumerable<User>> GetUserSupervisorsByIdAsync(Guid id);
        Task AddSupervisorAsync(Guid userId, Guid supervisorId);
        Task DeleteSupervisorAsync(Guid userId, Guid supervisorId);
    }
}
