using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Enums;

namespace ExpensesReport.Identity.Application.ViewModels
{
    public class IdentityViewModel(Guid id, string email, UserIdentityRole role, bool isDeleted)
    {
        public Guid Id { get; set; } = id;
        public string Email { get; set; } = email;
        public string RoleName { get; set; } = UserIdentityRoleExtensions.ToFriendlyString(role);
        public bool IsDeleted { get; set; } = isDeleted;

        public static IdentityViewModel FromEntity(UserIdentity identity, UserIdentityRole identityRole)
        {
            var identityId = Guid.Parse(identity.Id);

            return new IdentityViewModel(identityId, identity.Email!, identityRole, identity.IsDeleted);
        }
    }
}
