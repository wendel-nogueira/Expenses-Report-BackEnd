using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Repositories;
using MongoDB.Driver;

namespace ExpensesReport.Expenses.Infrastructure.Persistence.Repositories
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly string _collectionName = "Expenses";
        private readonly IMongoDatabase _mongoDatabase;

        public SignatureRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public IMongoCollection<ExpenseReport> GetCollection()
        {
            return _mongoDatabase.GetCollection<ExpenseReport>(_collectionName);
        }

        public async Task<IEnumerable<Signature>> GetAllAsync()
        {
            var expenseReports = await GetCollection().Find(_ => true).ToListAsync();

            return expenseReports.SelectMany(e => e.Signatures);
        }

        public async Task<IEnumerable<Signature>> GetAllInExpenseReportAsync(string expenseReportId)
        {
            var expenseReport = await GetCollection().Find(e => e.Id == expenseReportId).FirstOrDefaultAsync();

            return expenseReport.Signatures;
        }

        public async Task<Signature?> GetByIdAsync(string id)
        {
            var expenseReports = await GetCollection().Find(_ => true).ToListAsync();
            var signatures = expenseReports.SelectMany(e => e.Signatures).FirstOrDefault(s => s.Id == id);

            return signatures;
        }
    }
}
