using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;

namespace ExpensesReport.Expenses.Application.ViewModels
{
    public class ExpenseViewModel
    {
        public string? Id { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DateIncurred { get; set; }
        public string? Explanation { get; set; }
        public ExpenseStatus? Status { get; set; }
        public Guid? ActionById { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? AccountingNotes { get; set; }
        public string? Receipt { get; set; }
        public bool? IsDeleted { get; set; }

        public static ExpenseViewModel FromEntity(Expense expense)
            => new()
            {
                Id = expense.Id,
                Amount = expense.Amount,
                DateIncurred = expense.DateIncurred,
                Explanation = expense.Explanation,
                Status = expense.Status,
                ActionById = expense.ActionById,
                ActionDate = expense.ActionDate,
                AccountingNotes = expense.AccountingNotes,
                Receipt = expense.Receipt,
                IsDeleted = expense.IsDeleted
            };
    }
}
