namespace ExpensesReport.Expenses.Infrastructure.Persistence.Context
{
    public class ExpensesDbContext : IExpensesDbContext
    {
        public string ConnectionString { get; }
        public string Database { get; }

        public ExpensesDbContext(string connectionString, string database)
        {
            ConnectionString = connectionString;
            Database = database;
        }
    }
}
