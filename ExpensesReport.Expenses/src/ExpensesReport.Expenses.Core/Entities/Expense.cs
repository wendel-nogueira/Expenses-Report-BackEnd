using ExpensesReport.Expenses.Core.Enums;
using ExpensesReport.Users.Core.Entities;

namespace ExpensesReport.Expenses.Core.Entities
{
    public class Expense : EntityBase
    {
        public Expense(decimal amount, DateTime dateIncurred, string explanation, ExpenseStatus status, Guid actionById, DateTime actionDate, string accountingNotes, string receipt)
        {
            Amount = amount;
            DateIncurred = dateIncurred;
            Explanation = explanation;
            Status = status;
            ActionById = actionById;
            ActionDate = actionDate;
            AccountingNotes = accountingNotes;
            Receipt = receipt;
        }

        public decimal Amount { get; set; }
        public DateTime DateIncurred { get; set; }
        public string Explanation { get; set; }
        public ExpenseStatus Status { get; set; }
        public Guid ActionById { get; set; }
        public DateTime ActionDate { get; set; }
        public string AccountingNotes { get; set; }
        public string Receipt { get; set; }

        public void Update(decimal amount, DateTime dateIncurred, string explanation, ExpenseStatus status, string accountingNotes, string receipt)
        {
            Amount = amount;
            DateIncurred = dateIncurred;
            Explanation = explanation;
            Status = status;
            AccountingNotes = accountingNotes;
            Receipt = receipt;
            UpdatedAt = DateTime.Now;
        }
    }
}
