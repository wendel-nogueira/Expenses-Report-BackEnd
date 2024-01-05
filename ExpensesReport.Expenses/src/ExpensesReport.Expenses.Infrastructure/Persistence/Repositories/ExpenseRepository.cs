using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpensesReport.Expenses.Infrastructure.Persistence.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly string _collectionName = "Expenses";
        private readonly IMongoDatabase _mongoDatabase;

        public ExpenseRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public IMongoCollection<ExpenseReport> GetCollection()
        {
            return _mongoDatabase.GetCollection<ExpenseReport>(_collectionName);
        }

        public async Task<ExpenseReport?> GetExpenseReportByExpenseIdAsync(string expenseId)
        {
            var expenseReports = await GetCollection().Find(_ => true).ToListAsync();
            return expenseReports.FirstOrDefault(e => e.Expenses.Any(e => e.Id.ToString() == expenseId));
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            var expenseReports = await GetCollection().Find(_ => true).ToListAsync();

            return expenseReports.SelectMany(e => e.Expenses);
        }

        public async Task<Expense?> GetByIdAsync(string id)
        {
            var expenseReports = await GetCollection().Find(_ => true).ToListAsync();

            return expenseReports.SelectMany(e => e.Expenses).FirstOrDefault(s => s.Id.ToString() == id);
        }

        public async Task<IEnumerable<Expense>> GetAllInExpenseReportAsync(string expenseReportId)
        {
            var expenseReport = await GetCollection().Find(e => e.Id == expenseReportId).FirstOrDefaultAsync();

            return expenseReport.Expenses;
        }
    }
}
