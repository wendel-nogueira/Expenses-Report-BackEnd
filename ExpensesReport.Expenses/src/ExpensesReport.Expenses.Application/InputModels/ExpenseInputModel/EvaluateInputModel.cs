using System.ComponentModel.DataAnnotations;
using ExpensesReport.Expenses.Core.Enums;

namespace ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel
{
    public class EvaluateInputModel
    {
        [Required(ErrorMessage = "Status is required!")]
        [EnumDataType(typeof(ExpenseStatus))]
        public ExpenseStatus? Status { get; set; }

        [Required(ErrorMessage = "ActionBy is required!")]
        public string? ActionBy { get; set; }

        [Required(ErrorMessage = "ActionDate is required!")]
        [DataType(DataType.DateTime)]
        public DateTime? ActionDate { get; set; }

        [Required(ErrorMessage = "ActionDateTimeZone is required!")]
        public string? ActionDateTimeZone { get; set; }

        [Required(ErrorMessage = "AccountingNotes is required!")]
        public string? AccountingNotes { get; set; }
    }
}
