namespace ExpensesReport.Expenses.Core.Enums
{
    public enum ExpenseStatus
    {
        Approved = 0,
        Rejected = 1
    }

    public static class ExpenseStatusExtensions
    {
        public static string ToFriendlyString(ExpenseStatus? expenseStatus)
        {
            return expenseStatus switch
            {
                ExpenseStatus.Approved => "Approved",
                ExpenseStatus.Rejected => "Rejected",
                _ => "Unknown",
            };
        }


        public static ExpenseStatus ToEnum(this string status)
        {
            return status switch
            {
                "Approved" => ExpenseStatus.Approved,
                "Rejected" => ExpenseStatus.Rejected,
                _ => ExpenseStatus.Approved,
            };
        }
    }
}
