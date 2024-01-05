using ExpensesReport.Expenses.Core.Entities;

namespace ExpensesReport.Expenses.Core.Repositories
{
    public interface IExpenseRepository
    {
        public Task<ExpenseReport?> GetExpenseReportByExpenseIdAsync(string expenseId);
        public Task<IEnumerable<Expense>> GetAllAsync();
        public Task<Expense?> GetByIdAsync(string id);
        public Task<IEnumerable<Expense>> GetAllInExpenseReportAsync(string expenseReportId);
    }
}