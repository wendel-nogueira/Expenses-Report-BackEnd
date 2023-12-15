using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Repositories;
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
            return expenseReports.FirstOrDefault(e => e.Expenses.Any(e => e.Id == expenseId));
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            var expenseReports = await GetCollection().Find(_ => true).ToListAsync();

            return expenseReports.SelectMany(e => e.Expenses);
        }

        public async Task<Expense?> GetByIdAsync(string id)
        {
            var expenseReports = await GetCollection().Find(_ => true).ToListAsync();

            return expenseReports.SelectMany(e => e.Expenses).FirstOrDefault(s => s.Id == id);
        }

        public async Task<IEnumerable<Expense>> GetAllInExpenseReportAsync(string expenseReportId)
        {
            var expenseReport = await GetCollection().Find(e => e.Id == expenseReportId).FirstOrDefaultAsync();

            return expenseReport.Expenses;
        }

        public async Task<Expense> AddAsync(string expenseId, Expense expense)
        {
            var expenseReport = await GetCollection().Find(e => e.Id == expenseId).FirstOrDefaultAsync();

            expenseReport.AddExpense(expense);
            await GetCollection().ReplaceOneAsync(e => e.Id == expenseReport.Id, expenseReport);

            return expense;
        }

        public async Task<Expense> UpdateAsync(string expenseId, Expense expense)
        {
            var expenseReport = await GetCollection().Find(e => e.Id == expenseId).FirstOrDefaultAsync();
            var expenseToUpdate = expenseReport.Expenses.FirstOrDefault(e => e.Id == expense.Id);

            expenseReport.RemoveExpense(expenseToUpdate!);
            expenseReport.AddExpense(expense);

            await GetCollection().ReplaceOneAsync(e => e.Id == expenseReport.Id, expenseReport);

            return expense;
        }

        public async Task DeleteAsync(string expenseReportId, Expense expense)
        {
            var expenseReport = await GetCollection().Find(e => e.Id == expenseReportId).FirstOrDefaultAsync();

            expenseReport.RemoveExpense(expense!);

            await GetCollection().ReplaceOneAsync(e => e.Id == expenseReport.Id, expenseReport);
        }
    }
}
