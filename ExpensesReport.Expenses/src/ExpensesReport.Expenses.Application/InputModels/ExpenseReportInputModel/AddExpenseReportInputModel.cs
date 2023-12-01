using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Expenses.Application.InputModels.ExpenseReportInputModel
{
    public class AddExpenseReportInputModel
    {
        [Required(ErrorMessage = "UserId is required!")]
        public Guid? UserId { get; set; }

        [Required(ErrorMessage = "DepartmentId is required!")]
        public Guid? DepartmentId { get; set; }

        [Required(ErrorMessage = "ProjectId is required!")]
        public Guid? ProjectId { get; set; }

        [Required(ErrorMessage = "Status is required!")]
        [EnumDataType(typeof(ExpenseReportStatus), ErrorMessage = "Status must be a valid value!")]
        public ExpenseReportStatus? Status { get; set; }

        [Required(ErrorMessage = "TotalAmount is required!")]
        [Range(0, 99999.99, ErrorMessage = "TotalAmount must be a valid value!")]
        public decimal? TotalAmount { get; set; }

        [Required(ErrorMessage = "StatusNotes is required!")]
        [StringLength(100, ErrorMessage = "StatusNotes must be less than 100 characters!")]
        public string? StatusNotes { get; set; }

        public ExpenseReport ToEntity() => new(UserId!.Value, DepartmentId!.Value, ProjectId!.Value, Status!.Value, TotalAmount!.Value, null, null, null, null, null, StatusNotes!, null);
    }
}