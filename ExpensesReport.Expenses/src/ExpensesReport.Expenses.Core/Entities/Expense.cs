using ExpensesReport.Expenses.Core.Enums;
using ExpensesReport.Users.Core.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ExpensesReport.Expenses.Core.Entities
{
    public class Expense : EntityBase
    {
        public Expense(string expenseAccount, decimal amount, DateTime dateIncurred, string explanation, ExpenseStatus? status, Guid? actionById, DateTime? actionDate, string? accountingNotes, string receipt, string? dateIncurredTimeZone, string? actionDateTimeZone)
        {
            ExpenseAccount = expenseAccount;
            Amount = amount;
            DateIncurred = dateIncurred;
            DateIncurredTimeZone = dateIncurredTimeZone;
            Explanation = explanation;
            Status = status;
            ActionById = actionById;
            ActionDate = actionDate;
            ActionDateTimeZone = actionDateTimeZone;
            AccountingNotes = accountingNotes;
            Receipt = receipt;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ExpenseAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateIncurred { get; set; }
        public string? DateIncurredTimeZone { get; set; }
        public string Explanation { get; set; }
        public ExpenseStatus? Status { get; set; }
        public Guid? ActionById { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? ActionDateTimeZone { get; set; }
        public string? AccountingNotes { get; set; }
        public string Receipt { get; set; }


        public void Update(string expenseAccount, decimal amount, DateTime dateIncurred, string explanation, ExpenseStatus status, string accountingNotes, string receipt, string? dateIncurredTimeZone)
        {
            ExpenseAccount = expenseAccount;
            Amount = amount;
            DateIncurred = dateIncurred;
            DateIncurredTimeZone = dateIncurredTimeZone;
            Explanation = explanation;
            Status = status;
            AccountingNotes = accountingNotes;
            Receipt = receipt;
            UpdatedAt = DateTime.Now;
        }
    }
}
