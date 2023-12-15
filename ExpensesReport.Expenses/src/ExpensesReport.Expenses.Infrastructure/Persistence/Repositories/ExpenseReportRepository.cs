using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Repositories;
using MongoDB.Driver;

namespace ExpensesReport.Expenses.Infrastructure.Persistence.Repositories
{
    public class ExpenseReportRepository : IExpenseReportRepository
    {
        private readonly string _collectionName = "Expenses";
        private readonly IMongoDatabase _mongoDatabase;

        public ExpenseReportRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public IMongoCollection<ExpenseReport> GetCollection()
        {
            return _mongoDatabase.GetCollection<ExpenseReport>(_collectionName);
        }

        public async Task<IEnumerable<ExpenseReport>> GetAllAsync()
        {
            return await GetCollection().Find(_ => true).ToListAsync();
        }

        public async Task<ExpenseReport> GetByIdAsync(string id)
        {
            return await GetCollection().Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ExpenseReport>> GetByUserAsync(Guid userId)
        {
            return await GetCollection().Find(e => e.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<ExpenseReport>> GetByDepartamentAsync(Guid departamentId)
        {
            return await GetCollection().Find(e => e.DepartamentId == departamentId).ToListAsync();
        }

        public async Task<IEnumerable<ExpenseReport>> GetByProjectAsync(Guid projectId)
        {
            return await GetCollection().Find(e => e.ProjectId == projectId).ToListAsync();
        }

        public async Task<ExpenseReport> AddAsync(ExpenseReport expenseReport)
        {
            await GetCollection().InsertOneAsync(expenseReport);

            return expenseReport;
        }

        public async Task<ExpenseReport> UpdateAsync(ExpenseReport expenseReport)
        {
            await GetCollection().ReplaceOneAsync(e => e.Id == expenseReport.Id, expenseReport);
            return expenseReport;
        }
    }
}
