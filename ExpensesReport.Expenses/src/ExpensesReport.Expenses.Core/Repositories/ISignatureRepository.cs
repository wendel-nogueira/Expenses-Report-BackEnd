using ExpensesReport.Expenses.Core.Entities;

namespace ExpensesReport.Expenses.Core.Repositories
{
    public interface ISignatureRepository
    {
        public Task<IEnumerable<Signature>> GetAllAsync();
        public Task<IEnumerable<Signature>> GetAllInExpenseReportAsync(string expenseReportId);
        public Task<Signature?> GetByIdAsync(string id);
        public Task<Signature> AddAsync(string expenseReportId, Signature signature);
    }
}