namespace ExpensesReport.Export.Core.Entities
{
    public class ExpenseAccount
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
