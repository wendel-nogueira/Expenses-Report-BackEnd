using ExpensesReport.Expenses.Core.Entities;

namespace ExpensesReport.Expenses.Core.Repositories
{
    public interface IExpenseAccountRepository
    {
        public Task<IEnumerable<ExpenseAccount>> GetAllAsync();
        public Task<ExpenseAccount> GetByIdAsync(string id);
        public Task<ExpenseAccount> GetByCodeAsync(string code);
        public Task<ExpenseAccount> AddAsync(ExpenseAccount expenseAccount);
        public Task<ExpenseAccount> UpdateAsync(ExpenseAccount expenseAccount);
    }
}
