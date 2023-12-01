using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;

namespace ExpensesReport.Expenses.Application.ViewModels
{
    public class ExpenseAccountViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public AccountType? Type { get; set; }
        public bool? IsDeleted { get; set; }

        public static ExpenseAccountViewModel FromEntity(ExpenseAccount expenseAccount)
            => new()
            {
                Id = expenseAccount.Id,
                Name = expenseAccount.Name,
                Code = expenseAccount.Code,
                Description = expenseAccount.Description,
                Type = AccountTypeExtensions.ToEnum(expenseAccount.Type.ToString()),
                IsDeleted = expenseAccount.IsDeleted
            };
    }
}
