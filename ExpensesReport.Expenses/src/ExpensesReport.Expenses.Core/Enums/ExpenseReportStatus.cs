namespace ExpensesReport.Expenses.Core.Enums
{
    public enum ExpenseReportStatus
    {
        Submitted = 0,
        ApprovedBySupervisor = 1,
        RejectedBySupervisor = 2,
        Paid = 3,
        PaymentRejected = 4
    }

    public static class ExpenseReportStatusExtensions
    {
        public static string ToFriendlyString(ExpenseReportStatus? expenseReportStatus)
        {
            return expenseReportStatus switch
            {
                ExpenseReportStatus.Submitted => "Submitted",
                ExpenseReportStatus.ApprovedBySupervisor => "Approved by Supervisor",
                ExpenseReportStatus.RejectedBySupervisor => "Rejected by Supervisor",
                ExpenseReportStatus.Paid => "Paid",
                ExpenseReportStatus.PaymentRejected => "Payment rejected",
                _ => "Submitted",
            };
        }

        public static ExpenseReportStatus ToEnum(this string status)
        {
            return status switch
            {
                "Submitted" => ExpenseReportStatus.Submitted,
                "Approved by Supervisor" => ExpenseReportStatus.ApprovedBySupervisor,
                "Rejected by Supervisor" => ExpenseReportStatus.RejectedBySupervisor,
                "Paid" => ExpenseReportStatus.Paid,
                "Payment rejected" => ExpenseReportStatus.PaymentRejected,
                _ => ExpenseReportStatus.Submitted,
            };
        }
    }
}
