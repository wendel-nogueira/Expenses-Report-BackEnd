namespace ExpensesReport.Identity.Core.Enums
{
    public enum UserIdentityRole
    {
        FieldStaff = 0,
        Manager = 1,
        Accountant = 2,
    }

    public static class UserIdentityRoleExtensions
    {
        public static string ToFriendlyString(this UserIdentityRole me)
        {
            return me switch
            {
                UserIdentityRole.FieldStaff => "FieldStaff",
                UserIdentityRole.Manager => "Manager",
                UserIdentityRole.Accountant => "Accountant",
                _ => "Unknown",
            };
        }

        public static UserIdentityRole ToEnum(this string me)
        {
            return me switch
            {
                "FieldStaff" => UserIdentityRole.FieldStaff,
                "Manager" => UserIdentityRole.Manager,
                "Accountant" => UserIdentityRole.Accountant,
                _ => UserIdentityRole.FieldStaff,
            };
        }

        public static UserIdentityRole[] GetValues()
        {
            return new UserIdentityRole[]
            {
                UserIdentityRole.FieldStaff,
                UserIdentityRole.Manager,
                UserIdentityRole.Accountant,
            };
        }
    }
}
