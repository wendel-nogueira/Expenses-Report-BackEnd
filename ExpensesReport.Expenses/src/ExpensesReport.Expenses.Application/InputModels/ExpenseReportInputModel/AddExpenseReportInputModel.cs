using ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel;
using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Expenses.Application.InputModels.ExpenseReportInputModel
{
    public class AddExpenseReportInputModel
    {
        [Required(ErrorMessage = "UserId is required!")]
        public Guid? UserId { get; set; }

        [Required(ErrorMessage = "DepartamentId is required!")]
        public Guid? DepartamentId { get; set; }

        [Required(ErrorMessage = "ProjectId is required!")]
        public Guid? ProjectId { get; set; }

        [Required(ErrorMessage = "TotalAmount is required!")]
        [Range(0, 99999.99, ErrorMessage = "TotalAmount must be a valid value!")]
        public decimal? TotalAmount { get; set; }

        public IEnumerable<AddExpenseInputModel>? Expenses { get; set; }

        public ExpenseReport ToEntity() => new(UserId!.Value, DepartamentId!.Value, ProjectId!.Value, ExpenseReportStatus.Submitted, TotalAmount!.Value, 0, 0, 0, null, null, "", null, null);
    }
}