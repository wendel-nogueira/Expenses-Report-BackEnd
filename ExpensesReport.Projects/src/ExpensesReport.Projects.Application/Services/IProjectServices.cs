using ExpensesReport.Projects.Application.InputModels;
using ExpensesReport.Projects.Application.ViewModels;

namespace ExpensesReport.Projects.Application.Services
{
    public interface IProjectServices
    {
        Task<IEnumerable<ProjectViewModel>> GetAllProjects();
        Task<ProjectViewModel?> GetProjectById(Guid id);
        Task<ProjectViewModel?> GetProjectByCode(string code);
        Task<IEnumerable<ProjectViewModel>> GetProjectsByDepartamentId(Guid departmentId);
        Task<ProjectViewModel> AddProject(AddProjectInputModel inputModel);
        Task UpdateProject(Guid id, ChangeProjectInputModel inputModel);
        Task DeleteProject(Guid id);
        Task ActivateProject(Guid id);
    }
}
