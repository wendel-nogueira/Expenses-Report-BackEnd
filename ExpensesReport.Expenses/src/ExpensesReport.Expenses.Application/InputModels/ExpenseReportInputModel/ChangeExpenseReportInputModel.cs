using ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel;
using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Expenses.Application.InputModels.ExpenseReportInputModel
{
    public class ChangeExpenseReportInputModel
    {
        [Required(ErrorMessage = "DepartamentId is required!")]
        public Guid? DepartamentId { get; set; }

        [Required(ErrorMessage = "ProjectId is required!")]
        public Guid? ProjectId { get; set; }

        [Required(ErrorMessage = "Status is required!")]
        [EnumDataType(typeof(ExpenseReportStatus), ErrorMessage = "Status must be a valid value!")]
        public ExpenseReportStatus? Status { get; set; }

        [Required(ErrorMessage = "AmountPaid is required!")]
        [Range(0, 99999.99, ErrorMessage = "AmountPaid must be a valid value!")]
        public decimal? AmountPaid { get; set; }

        [Required(ErrorMessage = "PaidById is required!")]
        public Guid? PaidById { get; set; }

        [Required(ErrorMessage = "PaidDate is required!")]
        [DataType(DataType.DateTime)]
        public DateTime? PaidDate { get; set; }

        [Required(ErrorMessage = "PaidDateTimeZone is required!")]
        public string? PaidDateTimeZone { get; set; }

        [Required(ErrorMessage = "StatusNotes is required!")]
        [StringLength(100, ErrorMessage = "StatusNotes must be less than 100 characters!")]
        public string? StatusNotes { get; set; }

        [Required(ErrorMessage = "ProofOfPayment is required!")]
        [StringLength(100, ErrorMessage = "ProofOfPayment must be less than 100 characters!")]
        public string? ProofOfPayment { get; set; }

        public ExpenseReport ToEntity(Guid userId, Guid departmentId, Guid projectId) => new(userId, departmentId, projectId, Status!.Value, 0, 0, 0, AmountPaid!.Value, PaidById!.Value, PaidDate!.Value, StatusNotes!, ProofOfPayment!, PaidDateTimeZone);
    }
}
