using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;

namespace ExpensesReport.Expenses.Application.ViewModels
{
    public class ExpenseReportViewModel
    {
        public string? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? ProjectId { get; set; }
        public ExpenseReportStatus? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? AmountApproved { get; set; }
        public decimal? AmountRejected { get; set; }
        public decimal? AmountPaid { get; set; }
        public Guid? PaidById { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? StatusNotes { get; set; }
        public string? ProofOfPayment { get; set; }
        public bool? IsDeleted { get; set; }

        public static ExpenseReportViewModel FromEntity(ExpenseReport expenseReport) => new()
        {
            Id = expenseReport.Id,
            UserId = expenseReport.UserId,
            DepartmentId = expenseReport.DepartmentId,
            ProjectId = expenseReport.ProjectId,
            Status = expenseReport.Status,
            TotalAmount = expenseReport.TotalAmount,
            AmountApproved = expenseReport.AmountApproved,
            AmountRejected = expenseReport.AmountRejected,
            AmountPaid = expenseReport.AmountPaid,
            PaidById = expenseReport.PaidById,
            PaidDate = expenseReport.PaidDate,
            StatusNotes = expenseReport.StatusNotes,
            ProofOfPayment = expenseReport.ProofOfPayment,
            IsDeleted = expenseReport.IsDeleted
        };
    }
}
