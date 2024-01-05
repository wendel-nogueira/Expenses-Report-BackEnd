namespace ExpensesReport.Export.Core.Entities
{
    public class Expense
    {
        public string? Id { get; set; }
        public string? ExpenseAccount { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DateIncurred { get; set; }
        public string? Explanation { get; set; }
        public int? Status { get; set; }
        public Guid? ActionById { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? AccountingNotes { get; set; }
        public string? Receipt { get; set; }
    }
}
