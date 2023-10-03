using ExpensesReport.Identity.Application.InputModels;
using ExpensesReport.Identity.Application.ViewModels;

namespace ExpensesReport.Identity.Application.Services
{
    public interface IIdentityServices
    {
        Task<IdentityViewModel> GetIdentityById(Guid id);
        Task<IdentityViewModel> GetIdentityByEmail(string email);
        Task<IdentityCheckViewModel> GetMe(string token);
        Task<IEnumerable<IdentityViewModel>> GetAll();
        Task<IEnumerable<IdentityViewModel>> GetAllByRole(string role);
        Task<IEnumerable<RoleViewModel>> GetAllRoles();
        Task<AuthenticationViewModel> Login(LoginInputModel inputModel);
        Task<IdentityViewModel> AddIdentity(AddIdentityInputModel inputModel);
        Task AddIdentityPassword(string token, ChangePasswordInputModel inputModel);
        Task UpdateIdentityPassword(Guid id, ChangePasswordInputModel inputModel);
        Task UpdateIdentityEmail(Guid id, ChangeEmailInputModel inputModel);
        Task UpdateIdentityRole(Guid id, ChangeIdentityRoleInputModel inputModel);
        Task DeleteIdentity(Guid id);
    }
}
