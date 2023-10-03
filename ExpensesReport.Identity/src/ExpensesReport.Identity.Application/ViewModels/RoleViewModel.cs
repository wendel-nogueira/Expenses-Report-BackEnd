using ExpensesReport.Identity.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExpensesReport.Identity.Application.ViewModels
{
    public class RoleViewModel(Guid id, string name, int enumId)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public int EnumId { get; set; } = enumId;

        public static RoleViewModel FromEntity(IdentityRole identityRole)
        {
            var identityRoleId = Guid.Parse(identityRole.Id);
            var enumId = UserIdentityRoleExtensions.ToEnum(identityRole.Name!).GetHashCode();

            return new RoleViewModel(identityRoleId, identityRole.Name!, enumId);
        }
    }
}
