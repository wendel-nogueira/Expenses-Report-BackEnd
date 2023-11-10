using ExpensesReport.Departaments.Application.InputModels;
using ExpensesReport.Departaments.Application.ViewModels;

namespace ExpensesReport.Departaments.Application.Services
{
    public interface IDepartamentServices
    {
        Task<DepartamentViewModel> GetDepartamentById(Guid id);
        Task<DepartamentViewModel> GetDepartamentByName(string name);
        Task<DepartamentViewModel> GetDepartamentByAcronym(string acronym);
        Task<IEnumerable<DepartamentViewModel>> GetAllDepartaments();
        Task<DepartamentViewModel> AddDepartament(AddDepartamentInputModel inputModel);
        Task UpdateDepartament(Guid id, ChangeDepartamentInputModel inputModel);
        Task ActivateDepartament(Guid id);
        Task DeactivateDepartament(Guid id);
        Task<ManagerViewModel> GetDepartamentManagers(Guid departamentId);
        Task AddDepartamentManager(Guid departamentId, Guid managerId);
        Task RemoveDepartamentManager(Guid departamentId, Guid managerId);
        Task<UserViewModel> GetDepartamentUsers(Guid departamentId);
        Task AddDepartamentUser(Guid departamentId, Guid userId);
        Task RemoveDepartamentUser(Guid departamentId, Guid userId);
        Task<IEnumerable<DepartamentViewModel>> GetDepartamentsByManagerAndUser(Guid id);
    }
}
