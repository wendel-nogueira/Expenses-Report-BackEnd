using Microsoft.AspNetCore.Identity;

namespace ExpensesReport.Identity.Application.ViewModels
{
    public class RoleViewModel(Guid id, string name)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;

        public static RoleViewModel FromEntity(IdentityRole identityRole)
        {
            var identityRoleId = Guid.Parse(identityRole.Id);

            return new RoleViewModel(identityRoleId, identityRole.Name!);
        }
    }
}
