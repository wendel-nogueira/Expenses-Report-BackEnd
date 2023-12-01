using ExpensesReport.Expenses.Core.Entities;

namespace ExpensesReport.Expenses.Core.Repositories
{
    public interface IExpenseReportRepository
    {
        public Task<IEnumerable<ExpenseReport>> GetAllAsync();
        public Task<ExpenseReport> GetByIdAsync(string id);
        public Task<IEnumerable<ExpenseReport>> GetByUserAsync(Guid userId);
        public Task<IEnumerable<ExpenseReport>> GetByDepartmentAsync(Guid departmentId);
        public Task<IEnumerable<ExpenseReport>> GetByProjectAsync(Guid projectId);
        public Task<ExpenseReport> AddAsync(ExpenseReport expenseReport);
        public Task<ExpenseReport> UpdateAsync(ExpenseReport expenseReport);
    }
}