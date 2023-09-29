using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Enums;

namespace ExpensesReport.Identity.Application.ViewModels
{
    public class IdentityViewModel(Guid id, string email, UserIdentityRole role)
    {
        public Guid Id { get; set; } = id;
        public string Email { get; set; } = email;
        public UserIdentityRole Role { get; set; } = role;

        public static IdentityViewModel FromEntity(UserIdentity identity, UserIdentityRole identityRole)
        {
            var identityId = Guid.Parse(identity.Id);

            return new IdentityViewModel(identityId, identity.Email!, identityRole);
        }
    }
}
