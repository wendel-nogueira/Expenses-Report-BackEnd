using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Enums;
using ExpensesReport.Identity.Core.repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Identity.Infrastructure.Persistence.Repositories
{
    public class UserIdentityRepository : IUserIdentityRepository
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserIdentityRepository(UserManager<UserIdentity> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserIdentity>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }

        public async Task<IEnumerable<UserIdentity>> GetAllByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            return users;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return roles;
        }

        public async Task<UserIdentity?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user;
        }

        public async Task<UserIdentity?> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<UserIdentity?> GetByResetPasswordTokenAsync(string token)
        {
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                if (user.ResetPasswordToken == token)
                    return user;
            }

            return null;
        }

        public async Task<IdentityRole?> GetRoleByIdentityIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            var userRole = await _userManager.GetRolesAsync(user);

            if (userRole == null)
                return null;

            var role = await _roleManager.FindByNameAsync(userRole.FirstOrDefault()!);

            return role;
        }

        public async Task<IdentityRole?> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            return role;
        }

        public async Task<IdentityRole?> GetRoleByNameAsync(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);

            return role;
        }

        public async Task<IdentityResult> AddAsync(UserIdentity identity, IdentityRole role)
        {
            var result = await _userManager.CreateAsync(identity);

            if (!result.Succeeded)
                return result;

            result = await _userManager.AddToRoleAsync(identity, role.Name!);

            return result;
        }

        public async Task<IdentityResult> UpdateIdentityAsync(UserIdentity identity)
        {
            var result = await _userManager.UpdateAsync(identity);

            return result;
        }

        public async Task<IdentityResult> UpdateIdentityRoleAsync(UserIdentity identity, UserIdentityRole role)
        {
            var user = await _userManager.FindByIdAsync(identity.Id);

            if (user == null)
                return IdentityResult.Failed();

            var userCurrentRole = await _userManager.GetRolesAsync(user);

            if (userCurrentRole == null)
                return IdentityResult.Failed();

            var result = await _userManager.RemoveFromRoleAsync(user, userCurrentRole.FirstOrDefault()!);

            if (!result.Succeeded)
                return result;

            var newRole = UserIdentityRoleExtensions.ToFriendlyString(role);
            result = await _userManager.AddToRoleAsync(user, newRole);

            return result;
        }

        public async Task<IdentityResult> ActivateAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return IdentityResult.Failed();

            user.Activate();

            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return IdentityResult.Failed();

            user.Delete();

            var result = await _userManager.UpdateAsync(user);

            return result;
        }
    }
}
