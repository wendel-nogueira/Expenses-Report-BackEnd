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

        [Required(ErrorMessage = "Expenses is required!")]
        [MinLength(1, ErrorMessage = "Expenses must have at least one item!")]
        [MaxLength(100, ErrorMessage = "Expenses must have a maximum of 100 items!")]
        public IEnumerable<AddExpenseInputModel>? Expenses { get; set; }

        public ExpenseReport ToEntity() => new(UserId!.Value, DepartamentId!.Value, ProjectId!.Value, ExpenseReportStatus.Submitted, 0, 0, 0, 0, null, null, "", null, null);
    }
}