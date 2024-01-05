namespace ExpensesReport.Export.Core.Entities
{
    public class ExpenseReport
    {
        public string? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? DepartamentId { get; set; }
        public Guid? ProjectId { get; set; }
        public int? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? AmountApproved { get; set; }
        public decimal? AmountRejected { get; set; }
        public decimal? AmountPaid { get; set; }
        public Guid? PaidById { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? StatusNotes { get; set; }
        public string? ProofOfPayment { get; set; }
        public bool? IsDeleted { get; set; }
        public IEnumerable<Expense>? Expenses { get; set; }
        public IEnumerable<Signature>? Signatures { get; set; }
    }
}
