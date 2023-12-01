namespace ExpensesReport.Expenses.Infrastructure.Persistence.Context
{
    public interface IExpensesDbContext
    {
        public string ConnectionString { get; }
        public string Database { get; }
    }
}
