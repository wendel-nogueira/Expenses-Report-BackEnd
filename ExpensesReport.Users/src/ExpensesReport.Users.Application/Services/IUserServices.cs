using ExpensesReport.Users.Application.InputModels;
using ExpensesReport.Users.Application.ViewModels;

namespace ExpensesReport.Users.Application.Services
{
    public interface IUserServices
    {
        Task<UserViewModel> GetUserById(Guid id);
        Task<UserViewModel> GetUserByIdentityId(Guid id);
        Task<IEnumerable<UserViewModel>> GetUsers();
        Task<UserViewModel> AddUser(AddUserInputModel inputModel);
        Task<UserViewModel> UpdateUser(Guid id, UpdateUserInputModel inputModel);
        Task<IEnumerable<UserViewModel>> GetUserSupervisorsById(Guid id);
        Task<UserViewModel> AddUserSupervisor(Guid id, Guid supervisorId);
        Task<UserViewModel> DeleteUserSupervisor(Guid id, Guid supervisorId);
    }
}
