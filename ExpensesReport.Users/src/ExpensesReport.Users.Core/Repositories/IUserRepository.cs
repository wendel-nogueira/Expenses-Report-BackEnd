using ExpensesReport.Users.Core.Entities;

namespace ExpensesReport.Users.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(Guid id, User userUpdate);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<User>> GetUserSupervisorsByIdAsync(Guid id);
        Task AddSupervisorAsync(Guid userId, Guid supervisorId);
        Task DeleteSupervisorAsync(Guid userId, Guid supervisorId);
    }
}
