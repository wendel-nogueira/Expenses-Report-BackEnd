using ExpensesReport.Projects.Core.Entities;

namespace ExpensesReport.Projects.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project?> GetByCodeAsync(string code);
        Task<IEnumerable<Project>> GetProjectsByDepartamentIdAsync(Guid departamentId);
        Task<Project> AddAsync(Project project);
        Task<Project> UpdateAsync(Project project);
        Task DeactivateAsync(Project project);
        Task ActivateAsync(Project project);
    }
}
