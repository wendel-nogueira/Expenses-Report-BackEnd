namespace ExpensesReport.Expenses.Core.Enums
{
    public enum AccountType
    {
        Asset = 0,
        Expense = 1
    }

    public static class AccountTypeExtensions
    {
        public static string ToFriendlyString(AccountType? accountType)
        {
            return accountType switch
            {
                AccountType.Asset => "Asset",
                AccountType.Expense => "Expense",
                _ => "Unknown",
            };
        }

        public static AccountType ToEnum(this string type)
        {
            return type switch
            {
                "Asset" => AccountType.Asset,
                "Expense" => AccountType.Expense,
                _ => AccountType.Asset,
            };
        }
    }
}
