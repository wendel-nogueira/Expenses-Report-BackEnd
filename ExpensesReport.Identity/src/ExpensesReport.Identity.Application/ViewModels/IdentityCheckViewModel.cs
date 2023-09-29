using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Enums;

namespace ExpensesReport.Identity.Application.ViewModels
{
    public class IdentityCheckViewModel
    {
        public IdentityCheckViewModel(Guid id, string email, UserIdentityRole role, List<string> operations)
        {
            Id = id;
            Email = email;
            Role = role;
            Operations = operations;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public UserIdentityRole Role { get; set; }
        public List<string> Operations { get; set; }

        public static IdentityCheckViewModel FromEntity(UserIdentity identity, UserIdentityRole identityRole, List<string> permissions)
        {
            var identityId = Guid.Parse(identity.Id);

            return new IdentityCheckViewModel(identityId, identity.Email!, identityRole, permissions);
        }
    }
}
