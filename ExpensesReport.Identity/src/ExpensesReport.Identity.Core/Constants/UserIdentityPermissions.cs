using ExpensesReport.Identity.Core.Enums;

namespace ExpensesReport.Identity.Core.Constants
{
    public static class UserIdentityPermissions
    {
        static readonly Dictionary<string, List<string>> PermissionsList = new()
        {
            {
                UserIdentityRole.FieldStaff.ToString(), new List<string> {
                    //  Identity operations
                    "identity:me",
                    "identity:getAllRoles",
                    "identity:changePassword",
                    "identity:resetPassword",
                    "identity:updateEmail",

                    //  User operations
                    //  Departament operations
                    //  Project operations
                    //  Expense operations
                    //  Export operations
                    //  File operations
                }
            },
            {
                UserIdentityRole.Manager.ToString(), new List<string> {
                    //  Identity operations
                    "identity:me",
                    "identity:getAllRoles",
                    "identity:changePassword",
                    "identity:resetPassword",
                    "identity:updateEmail",
                    "identity:getByIdentityId",
                    "identity.addIdentity",

                    //  User operations
                    //  Departament operations
                    //  Project operations
                    //  Expense operations
                    //  Export operations
                    //  File operations
                }
            },
            {
                UserIdentityRole.Accountant.ToString(), new List<string> {
                    //  Identity operations
                    "identity:me",
                    "identity:getAllRoles",
                    "identity:changePassword",
                    "identity:resetPassword",
                    "identity:updateEmail",
                    "identity:getByIdentityId",
                    "identity:getByIdentityEmail",
                    "identity:getAll",
                    "identity:getAllByRole",
                    "identity.addIdentity",
                    "identity:updateRole",
                    "identity:delete",
                    
                    //  User operations
                    //  Departament operations
                    //  Project operations
                    //  Expense operations
                    //  Export operations
                    //  File operations
                }
            },
        };

        public static List<string> GetPermissions(string role) => PermissionsList[role];
    }
}
