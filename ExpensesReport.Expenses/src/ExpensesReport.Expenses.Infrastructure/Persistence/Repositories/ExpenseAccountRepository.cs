using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Repositories;
using MongoDB.Driver;

namespace ExpensesReport.Expenses.Infrastructure.Persistence.Repositories
{
    public class ExpenseAccountRepository : IExpenseAccountRepository
    {
        private readonly string _collectionName = "ExpenseAccounts";
        private readonly IMongoDatabase _mongoDatabase;

        public ExpenseAccountRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public IMongoCollection<ExpenseAccount> GetCollection()
        {
            return _mongoDatabase.GetCollection<ExpenseAccount>(_collectionName);
        }

        public async Task<IEnumerable<ExpenseAccount>> GetAllAsync()
        {
            return await GetCollection().Find(_ => true).ToListAsync();
        }

        public async Task<ExpenseAccount> GetByIdAsync(string id)
        {
            return await GetCollection().Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ExpenseAccount> GetByCodeAsync(string code)
        {
            return await GetCollection().Find(e => e.Code == code).FirstOrDefaultAsync();
        }

        public async Task<ExpenseAccount> AddAsync(ExpenseAccount expenseAccount)
        {
            await GetCollection().InsertOneAsync(expenseAccount);
            return expenseAccount;
        }

        public async Task<ExpenseAccount> UpdateAsync(ExpenseAccount expenseAccount)
        {
            await GetCollection().ReplaceOneAsync(e => e.Id == expenseAccount.Id, expenseAccount);
            return expenseAccount;
        }
    }
}
