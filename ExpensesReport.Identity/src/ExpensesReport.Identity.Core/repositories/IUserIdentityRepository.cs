using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExpensesReport.Identity.Core.repositories
{
    public interface IUserIdentityRepository
    {
        Task<IEnumerable<UserIdentity>> GetAllAsync();
        Task<IEnumerable<UserIdentity>> GetAllByRoleAsync(string role);
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<UserIdentity?> GetByIdAsync(string id);
        Task<UserIdentity?> GetByEmailAsync(string email);
        Task<UserIdentity?> GetByResetPasswordTokenAsync(string token);
        Task<IdentityRole?> GetRoleByIdentityIdAsync(string id);
        Task<IdentityRole?> GetRoleByIdAsync(string id);
        Task<IdentityRole?> GetRoleByNameAsync(string name);
        Task<IdentityResult> AddAsync(UserIdentity identity, IdentityRole role);
        Task<IdentityResult> UpdateIdentityAsync(UserIdentity identity);
        Task<IdentityResult> UpdateIdentityRoleAsync(UserIdentity identity, UserIdentityRole role);
        Task<IdentityResult> ActivateAsync(string id);
        Task<IdentityResult> DeleteAsync(string id);
    }
}
