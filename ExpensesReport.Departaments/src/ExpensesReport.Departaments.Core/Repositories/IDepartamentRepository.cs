using ExpensesReport.Departaments.Core.Entities;

namespace ExpensesReport.Departaments.Core.Repositories
{
    public interface IDepartamentRepository
    {
        Task<Departament?> GetByIdAsync(Guid id);
        Task<Departament?> GetByNameAsync(string name);
        Task<Departament?> GetByAcronymAsync(string acronym);
        Task<IEnumerable<Departament>> GetAllAsync();
        Task AddAsync(Departament departament);
        Task UpdateAsync(Departament departament);
        Task ActivateAsync(Departament departament);
        Task DeactivateAsync(Departament departament);
        Task<IEnumerable<Manager>> GetAllManagersAsync(Guid id);
        Task AddManagerAsync(Guid departamentId, Guid managerId);
        Task RemoveManagerAsync(Guid departamentId, Guid managerId);
        Task<IEnumerable<User>> GetAllUsersAsync(Guid id);
        Task AddUserAsync(Guid departamentId, Guid userId);
        Task RemoveUserAsync(Guid departamentId, Guid userId);
        Task<IEnumerable<Departament>> GetDepartamentsByManagerAndUser(Guid id);
    }
}
